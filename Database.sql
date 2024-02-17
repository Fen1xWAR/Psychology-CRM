create table if not exists clients
(
client_id    uuid not null
constraint clients_pk
primary key,
name         varchar,
lastname     varchar,
email        varchar,
phone_number varchar
);

alter table clients
owner to postgres;

create table if not exists visits
(
visit_id    uuid not null
constraint visits_pk
primary key,
client_id   uuid
constraint visits_clients_client_id_fk
references clients,
date_time   timestamp,
client_note varchar
);

alter table visits
owner to postgres;

create table if not exists visits_result
(
result_id uuid not null
constraint visits_result_pk
primary key,
visit_id  uuid
constraint visits_result_visits_visit_id_fk
references visits,
results   varchar
);

alter table visits_result
owner to postgres;

create table if not exists psychologists
(
" psychologist_id" uuid not null
constraint psychologists_pk
primary key,
lastname           varchar,
email              varchar,
phone_number       varchar,
name               varchar
);

alter table psychologists
owner to postgres;

create table if not exists visit_history
(
history_id        uuid not null
constraint visit_history_pk
primary key,
client_id         uuid
constraint visit_history_clients_client_id_fk
references clients,
psychologist_id   uuid
constraint "visit_history_psychologists_ psychologist_id_fk"
references psychologists,
date_time         timestamp,
psychologist_note text
);

alter table visit_history
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
references psychologists,
file_name       varchar,
file_content    bytea
);

alter table files
owner to postgres;

