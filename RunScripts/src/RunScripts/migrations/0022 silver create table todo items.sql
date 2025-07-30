CREATE TABLE silver.todo_items (
    id uuid NOT NULL,
    user_id uuid NOT NULL,
    description text NOT NULL,
    due_date timestamp with time zone,
    labels text[] NOT NULL,
    is_completed boolean NOT NULL,
    created_at timestamp with time zone NOT NULL,
    completed_at timestamp with time zone,
    priority integer NOT NULL,
    CONSTRAINT pk_todo_items PRIMARY KEY (id),
    CONSTRAINT fk_todo_items_users_user_id FOREIGN KEY (user_id) REFERENCES silver.users (id) ON DELETE CASCADE
);

CREATE INDEX ix_todo_items_user_id ON silver.todo_items (user_id);