CREATE OR REPLACE TRIGGER yaniv.TABLE1_TRG 
BEFORE INSERT ON yaniv.TABLE1 
FOR EACH ROW 
BEGIN
  <<COLUMN_SEQUENCES>>
  BEGIN
    IF INSERTING AND :NEW.COLUMN1 IS NULL THEN
      SELECT TABLE1_SEQ.NEXTVAL INTO :NEW.COLUMN1 FROM SYS.DUAL;
    END IF;
  END COLUMN_SEQUENCES;
END;
/