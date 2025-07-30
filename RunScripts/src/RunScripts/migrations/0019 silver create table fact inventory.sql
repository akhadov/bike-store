CREATE TABLE silver.fact_inventory (
    store_id integer NOT NULL,
    product_id integer NOT NULL,
    quantity integer NOT NULL,
    CONSTRAINT pk_fact_inventory PRIMARY KEY (store_id, product_id)
);