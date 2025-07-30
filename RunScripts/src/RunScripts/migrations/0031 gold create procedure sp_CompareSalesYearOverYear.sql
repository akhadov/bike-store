CREATE TABLE IF NOT EXISTS gold.yoy_sales_comparison (
    year INT,
    total_revenue NUMERIC,
    total_orders INT,
    generated_at TIMESTAMPTZ DEFAULT CURRENT_TIMESTAMP
);

CREATE OR REPLACE PROCEDURE gold.sp_CompareSalesYearOverYear(year1 INT, year2 INT)
LANGUAGE plpgsql
AS $$
BEGIN
    DELETE FROM gold.yoy_sales_comparison;

    INSERT INTO gold.yoy_sales_comparison (year, total_revenue, total_orders)
    SELECT EXTRACT(YEAR FROM order_date)::INT AS sales_year,
           SUM(total_price),
           COUNT(DISTINCT order_id)
    FROM silver.fact_sales
    WHERE EXTRACT(YEAR FROM order_date)::INT IN (year1, year2)
    GROUP BY sales_year;
END;
$$;
