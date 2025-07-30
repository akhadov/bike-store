CREATE TABLE bronze.stocks (
    store_id integer NOT NULL,
    product_id integer NOT NULL,
    quantity integer NOT NULL DEFAULT 0,
    CONSTRAINT pk_stocks PRIMARY KEY (store_id, product_id),
    CONSTRAINT fk_stocks_products_product_id FOREIGN KEY (product_id) REFERENCES bronze.products (product_id) ON DELETE CASCADE,
    CONSTRAINT fk_stocks_stores_store_id FOREIGN KEY (store_id) REFERENCES bronze.stores (store_id) ON DELETE CASCADE
);

CREATE INDEX ix_stocks_product_id ON bronze.stocks (product_id);