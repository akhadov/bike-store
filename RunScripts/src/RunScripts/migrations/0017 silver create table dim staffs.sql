CREATE TABLE silver.dim_staffs (
    staff_id integer GENERATED BY DEFAULT AS IDENTITY,
    first_name text NOT NULL,
    last_name text NOT NULL,
    email text NOT NULL,
    phone text,
    active boolean NOT NULL,
    store_id integer NOT NULL,
    manager_id integer,
    CONSTRAINT pk_dim_staffs PRIMARY KEY (staff_id),
    CONSTRAINT fk_dim_staffs_dim_staffs_manager_id FOREIGN KEY (manager_id) REFERENCES silver.dim_staffs (staff_id)
);

CREATE INDEX ix_dim_staffs_manager_id ON silver.dim_staffs (manager_id);