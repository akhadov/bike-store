CREATE OR REPLACE VIEW gold.vw_InventoryStatus AS
SELECT
    st.store_id,
    st.store_name,
    p.product_id,
    p.product_name,
    fi.quantity
FROM silver.fact_inventory fi
JOIN silver.dim_stores st ON st.store_id = fi.store_id
JOIN silver.dim_products p ON p.product_id = fi.product_id
WHERE fi.quantity < 10  -- threshold value, can be parameterized later
ORDER BY fi.quantity ASC;
