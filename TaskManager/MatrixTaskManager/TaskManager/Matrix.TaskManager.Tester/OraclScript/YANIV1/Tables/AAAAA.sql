CREATE TABLE yaniv1.aaaaa (
  column1 NUMBER NOT NULL,
  column2 VARCHAR2(20 BYTE),
  column3 VARCHAR2(20 BYTE),
  column4 VARCHAR2(20 BYTE),
  column5 VARCHAR2(20 BYTE),
  CONSTRAINT aaaaa_pk PRIMARY KEY (column1) USING INDEX yaniv1.aaaaa_index1
);