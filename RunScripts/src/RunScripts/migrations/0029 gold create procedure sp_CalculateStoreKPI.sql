CREATE TABLE IF NOT EXISTS gold.store_kpis (
    store_id INT NOT NULL,
    store_name TEXT NOT NULL,
    total_orders INT,
    total_revenue NUMERIC,
    average_order_value NUMERIC,
    kpi_generated_at TIMESTAMPTZ DEFAULT CURRENT_TIMESTAMP
);

CREATE OR REPLACE PROCEDURE gold.sp_CalculateStoreKPI()
LANGUAGE plpgsql
AS $$
BEGIN
    DELETE FROM gold.store_kpis;
    INSERT INTO gold.store_kpis (store_id, store_name, total_orders, total_revenue, average_order_value)
    SELECT store_id, store_name, total_orders, total_revenue, average_order_value
    FROM gold.vw_StoreSalesSummary;
END;
$$;
