CREATE TABLE silver.fact_sales (
    order_id integer NOT NULL,
    item_id integer NOT NULL,
    order_date timestamp with time zone NOT NULL,
    shipped_date timestamp with time zone,
    store_id integer NOT NULL,
    staff_id integer NOT NULL,
    customer_id integer NOT NULL,
    product_id integer NOT NULL,
    quantity integer NOT NULL,
    list_price numeric NOT NULL,
    discount numeric NOT NULL,
    total_price numeric GENERATED ALWAYS AS (quantity * list_price * (1 - discount)) STORED NOT NULL,
    CONSTRAINT pk_fact_sales PRIMARY KEY (order_id, item_id)
);