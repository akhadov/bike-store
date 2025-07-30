CREATE OR REPLACE VIEW gold.vw_RegionalTrends AS
SELECT
    st.state,
    st.city,
    SUM(s.total_price) AS total_revenue
FROM silver.fact_sales s
JOIN silver.dim_stores st ON st.store_id = s.store_id
GROUP BY st.state, st.city
ORDER BY total_revenue DESC;
