CREATE OR REPLACE VIEW gold.vw_TopSellingProducts AS
SELECT
    p.product_id,
    p.product_name,
    SUM(s.quantity) AS total_quantity_sold,
    SUM(s.total_price) AS total_sales,
    RANK() OVER (ORDER BY SUM(s.total_price) DESC) AS sales_rank
FROM silver.fact_sales s
JOIN silver.dim_products p ON p.product_id = s.product_id
GROUP BY p.product_id, p.product_name;
