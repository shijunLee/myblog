drop table post;

drop table tag;

drop table user_info;

create table post (
post_id              integer             autoincrement           not null,
post_date            DATE,
post_title           VARCHAR(200),
post_sub_title       VARCHAR(200),
post_content         TEXT,
post_content_preview TEXT,
post_markdown        TEXT,
post_title_slug      VARCHAR(200),
author_display_name  VARCHAR(20),
author_email         VARCHAR(20),
user_id              integer,
primary key (post_id)
);

create table tag (
tag_id               INTEGER               autoincrement         not null,
tag                  VARCHAR(200),
post_id              INTEGER,
slug                 VARCHAR(200),
primary key (tag_id)
);

create table user_info (
user_id              integer                 autoincrement       not null,
user_name            VARCHAR(50),
user_email           VARCHAR(200),
user_phone           VARCHAR(200),
user_sign            VARCHAR(500),
user_photo           VARCHAR(500),
primary key (user_id)
);

