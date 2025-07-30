CREATE OR REPLACE VIEW gold.vw_SalesByCategory AS
SELECT
    c.category_name,
    SUM(s.quantity) AS total_quantity_sold,
    SUM(s.total_price) AS total_sales
FROM silver.fact_sales s
JOIN silver.dim_products p ON p.product_id = s.product_id
JOIN silver.dim_categories c ON c.category_id = p.category_id
GROUP BY c.category_name;
