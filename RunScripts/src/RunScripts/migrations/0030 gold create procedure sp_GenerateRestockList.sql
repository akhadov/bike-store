CREATE TABLE IF NOT EXISTS gold.restock_list (
    store_id INT,
    product_id INT,
    product_name TEXT,
    current_quantity INT,
    generated_at TIMESTAMPTZ DEFAULT CURRENT_TIMESTAMP
);

CREATE OR REPLACE PROCEDURE gold.sp_GenerateRestockList(threshold INT DEFAULT 10)
LANGUAGE plpgsql
AS $$
BEGIN
    DELETE FROM gold.restock_list;
    INSERT INTO gold.restock_list (store_id, product_id, product_name, current_quantity)
    SELECT
        fi.store_id,
        p.product_id,
        p.product_name,
        fi.quantity
    FROM silver.fact_inventory fi
    JOIN silver.dim_products p ON p.product_id = fi.product_id
    WHERE fi.quantity < threshold;
END;
$$;
