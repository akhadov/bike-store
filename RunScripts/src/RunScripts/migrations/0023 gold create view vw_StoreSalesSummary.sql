CREATE OR REPLACE VIEW gold.vw_StoreSalesSummary AS
SELECT
    s.store_id,
    st.store_name,
    COUNT(DISTINCT s.order_id) AS total_orders,
    SUM(s.total_price) AS total_revenue,
    ROUND(SUM(s.total_price)::numeric / NULLIF(COUNT(DISTINCT s.order_id), 0), 2) AS average_order_value
FROM silver.fact_sales s
JOIN silver.dim_stores st ON st.store_id = s.store_id
GROUP BY s.store_id, st.store_name;
