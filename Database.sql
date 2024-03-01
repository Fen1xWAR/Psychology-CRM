create table if not exists visits_result
(
    result_id uuid not null
    constraint visits_result_pk
    primary key,
    results   varchar
);

alter table visits_result
    owner to postgres;

create table if not exists forms
(
    form_id      uuid not null
    constraint forms_pk
    primary key,
    form_content text
);

alter table forms
    owner to postgres;

create table if not exists services
(
    service_id          uuid not null
    constraint services_pk
    primary key,
    service_name        varchar(30),
    service_price       money,
    servive_description text
    );

alter table services
    owner to postgres;

create table if not exists contacts
(
    contact_id   uuid not null
    constraint contacts_pk
    primary key,
    email        varchar,
    phone_number integer
);

alter table contacts
    owner to postgres;

create table if not exists clients
(
    client_id       uuid not null
    constraint clients_pk
    primary key,
    name            varchar,
    lastname        varchar,
    form            uuid
    constraint clients_forms_form_id_fk
    references forms,
    current_problem varchar,
    contact_id      uuid
    constraint clients_contacts_contact_id_fk
    references contacts
);

alter table clients
    owner to postgres;

create table if not exists psyphologists
(
    " psychologist_id" uuid not null
    constraint psyphologists_pk
    primary key,
    name               varchar,
    lastname           varchar,
    contact_id         uuid
    constraint psyphologists_contacts_contact_id_fk
    references contacts
);

alter table psyphologists
    owner to postgres;

create table if not exists visits
(
    visit_id        uuid not null
    constraint visits_pk
    primary key,
    client_id       uuid
    constraint visits_clients_client_id_fk
    references clients,
    date_time       timestamp,
    client_note     varchar,
    service_id      uuid
    constraint visits_services_service_id_fk
    references services,
    psyphologist_id uuid
    constraint "visits_psyphologists_ psychologist_id_fk"
    references psyphologists,
    visit_note      uuid
    constraint visits_visits_result_result_id_fk
    references visits_result
);

alter table visits
    owner to postgres;

create table if not exists files
(
    file_id         uuid not null
    constraint files_pk
    primary key,
    client_id       uuid
    constraint files_clients_client_id_fk
    references clients,
    psyphologist_id uuid
    constraint "files_psychologists_ psychologist_id_fk"
    references psyphologists,
    file_name       varchar,
    file_content    bytea
);

alter table files
    owner to postgres;

create table if not exists payments
(
    payment_id     uuid not null
    constraint payments_pk
    primary key,
    client_id      uuid
    constraint payments_clients_client_id_fk
    references clients,
    visit_id       uuid
    constraint payments_sessions_session_id_fk
    references visits,
    payment_date   date,
    payment_amount money,
    payment_method varchar
);

alter table payments
    owner to postgres;

create table if not exists schedules
(
    schedule_id     uuid not null
    constraint schedules_pk
    primary key,
    psychologist_id uuid
    constraint schedules_psychologists_psychologist_id_fk
    references psyphologists,
    work_day        date,
    start_time      time,
    end_time        time
);

alter table schedules
    owner to postgres;

