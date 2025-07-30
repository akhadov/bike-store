CREATE TABLE IF NOT EXISTS gold.customer_profiles (
    customer_id INT,
    customer_name TEXT,
    total_orders INT,
    total_spent NUMERIC,
    most_bought_product TEXT,
    generated_at TIMESTAMPTZ DEFAULT CURRENT_TIMESTAMP
);

CREATE OR REPLACE PROCEDURE gold.sp_GetCustomerProfile(target_customer_id INT)
LANGUAGE plpgsql
AS $$
BEGIN
    DELETE FROM gold.customer_profiles WHERE customer_id = target_customer_id;

    INSERT INTO gold.customer_profiles (customer_id, customer_name, total_orders, total_spent, most_bought_product)
    SELECT
        c.customer_id,
        c.first_name || ' ' || c.last_name AS customer_name,
        COUNT(DISTINCT fs.order_id),
        SUM(fs.total_price),
        (
            SELECT p.product_name
            FROM silver.fact_sales fs2
            JOIN silver.dim_products p ON p.product_id = fs2.product_id
            WHERE fs2.customer_id = c.customer_id
            GROUP BY p.product_name
            ORDER BY SUM(fs2.quantity) DESC
            LIMIT 1
        )
    FROM silver.fact_sales fs
    JOIN silver.dim_customers c ON c.customer_id = fs.customer_id
    WHERE c.customer_id = target_customer_id
    GROUP BY c.customer_id, c.first_name, c.last_name;
END;
$$;
