CREATE OR REPLACE VIEW gold.vw_StaffPerformance AS
SELECT
    sf.staff_id,
    sf.first_name || ' ' || sf.last_name AS staff_name,
    COUNT(DISTINCT s.order_id) AS total_orders_handled,
    SUM(s.total_price) AS total_revenue_handled
FROM silver.fact_sales s
JOIN silver.dim_staffs sf ON sf.staff_id = s.staff_id
GROUP BY sf.staff_id, sf.first_name, sf.last_name;
