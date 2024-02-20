CREATE PROCEDURE [dbo].[GetAmortizationSchedule] (
	@Loan DECIMAL(19,2) = 36000,
	@InterestPerAnnum DECIMAL(19,2) = 8,
	@NumberOfMonthlyPayments INT = 36,
	@RecycleInterestPerAnnum DECIMAL(19, 2) = 4.5,
	@RecycleNumberOfMonthlyPayments INT = 48
)
AS
BEGIN
	DECLARE @MonthlyInterestRate DECIMAL(19, 10);
	DECLARE @MonthlyPayment DECIMAL(19, 2);
	DECLARE @RecycleMonthlyInterestRate DECIMAL(19, 10);

	SET @MonthlyInterestRate = @InterestPerAnnum / 12 / 100;
	SET @RecycleMonthlyInterestRate = @RecycleInterestPerAnnum / 12 / 100;
	SET @MonthlyPayment = @Loan * @MonthlyInterestRate / (1 - POWER(1 + @MonthlyInterestRate, -@NumberOfMonthlyPayments));

	-- Recursively compose schedule for first 12 payments
	WITH PaymentSchedule AS (
		SELECT
			1 AS [Payment],
			@MonthlyPayment AS [PaymentAmount],
			@Loan * @MonthlyInterestRate AS [Interest],
			@MonthlyPayment - (@Loan * @MonthlyInterestRate) AS [Principal],
			CONVERT(DECIMAL(19, 2), @Loan - @MonthlyPayment + (@Loan * @MonthlyInterestRate)) AS [Balance],
			@InterestPerAnnum AS [PerAnnum]
		UNION ALL
		SELECT
			[Payment] + 1,
			[PaymentAmount],
			[Balance] * @MonthlyInterestRate,
			[PaymentAmount] - ([Balance] * @MonthlyInterestRate),
			CONVERT(DECIMAL(19, 2), [Balance] - [PaymentAmount] + ([Balance] * @MonthlyInterestRate)),
			[PerAnnum]
		FROM 
			[PaymentSchedule]
		WHERE 
			[Payment] < 12
	),
	-- Post-12th payment, loan basis changes, Montly Payment is recalculated
	LoanChangeTermBasis AS (
		SELECT 
			[Balance] AS [Loan],
			[Balance] * @RecycleMonthlyInterestRate / (1 - POWER(1 + @RecycleMonthlyInterestRate, -@RecycleNumberOfMonthlyPayments)) AS [PaymentAmount]
		FROM 
			[PaymentSchedule]
		WHERE 
			[Payment] = 12
	),
	-- Recursively compose a schedule for payments past 12 month, but no longer than provided recycle period
	RecyclePaymentSchedule AS (
		SELECT
			13 AS [Payment],
			[LoanChangeTermBasis].[PaymentAmount] AS [PaymentAmount],
			CONVERT(DECIMAL(19, 10), [LoanChangeTermBasis].[PaymentAmount] * @RecycleMonthlyInterestRate) AS [Interest],
			[LoanChangeTermBasis].[PaymentAmount] - [LoanChangeTermBasis].[Loan] * @RecycleMonthlyInterestRate AS [Principal],
			CONVERT(DECIMAL(19, 2), [LoanChangeTermBasis].[Loan] - [LoanChangeTermBasis].[PaymentAmount] + [LoanChangeTermBasis].[Loan] * @RecycleMonthlyInterestRate) AS [Balance],
			@RecycleInterestPerAnnum AS [PerAnnum]
		FROM
			[LoanChangeTermBasis]
		UNION ALL
		SELECT 
			[Payment] + 1,
			[PaymentAmount],
			CONVERT(DECIMAL(19, 10), [Balance] * @RecycleMonthlyInterestRate),
			[PaymentAmount] - ([Balance] * @RecycleMonthlyInterestRate),
			CONVERT(DECIMAL(19, 2), [Balance] - [PaymentAmount] + ([Balance] * @RecycleMonthlyInterestRate)),
			[PerAnnum]
		FROM 
			[RecyclePaymentSchedule]
		WHERE
			[Payment] < @RecycleNumberOfMonthlyPayments + 12
	)
	
	SELECT 
		[Payment],
		[PaymentAmount],
		[Principal],
		[Interest],
		[Balance],
		[PerAnnum]
	FROM [PaymentSchedule]
	UNION ALL
	SELECT
		[Payment],
		[PaymentAmount],
		[Principal],
		[Interest],
		[Balance],
		[PerAnnum]
	FROM [RecyclePaymentSchedule]
END

