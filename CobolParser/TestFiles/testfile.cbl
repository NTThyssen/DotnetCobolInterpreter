       
       IDENTIFICATION DIVISION.
       PROGRAM-ID. HELLO-WORLD.
      
       ENVIRONMENT DIVISION.
      
       DATA DIVISION.
       WORKING-STORAGE SECTION.
       01  EMPLOYEE-RECORD.
           05  EMPLOYEE-ID              PIC 9(6).
           05  EMPLOYEE-NAME.
               10  FIRST-NAME           PIC X(15).
               10  MIDDLE-INITIAL       PIC X.
               10  LAST-NAME            PIC X(20).
           05  EMPLOYEE-ADDRESS.
               10  STREET-ADDRESS       PIC X(30).
               10  CITY                 PIC X(20).
               10  STATE                PIC XX.
               10  ZIP-CODE             PIC 9(5).
               10  ZIP-EXTENSION        PIC 9(4).
           05  DATE-OF-BIRTH            PIC 9(8).
           05  EMPLOYEE-PHONE           PIC X(12).
           05  DEPARTMENT-CODE          PIC X(4).
           05  SALARY                   PIC 9(7)V99.
           05  EMPLOYEE-STATUS          PIC X(1).
           05  HIRE-DATE                PIC 9(8).
           05  TERMINATION-DATE         PIC 9(8).
           05  EMPLOYMENT-HISTORY.
               10  JOB-TITLE            PIC X(20).
               10  JOB-DURATION         PIC 9(3).
               10  JOB-LOCATION         PIC X(15).
           05  EMPLOYEE-BENEFITS.
               10  HEALTH-PLAN          PIC X(1).
               10  DENTAL-PLAN          PIC X(1).
               10  VISION-PLAN          PIC X(1).
               10  RETIREMENT-PLAN      PIC X(1).
               10  LIFE-INSURANCE       PIC 9(5).
           05  DEPENDENT-INFO OCCURS 3 TIMES.
               10  DEPENDENT-NAME       PIC X(20).
               10  DEPENDENT-RELATION   PIC X(10).
               10  DEPENDENT-AGE        PIC 9(2).
           05  EMPLOYEE-SKILLS.
               10  SKILL OCCURS 5 TIMES.
                   15  SKILL-NAME       PIC X(15).
                   15  SKILL-LEVEL      PIC 9(1).
           05  PERFORMANCE-REVIEWS OCCURS 2 TIMES.
               10  REVIEW-DATE          PIC 9(8).
               10  REVIEW-RATING        PIC X(1).
               10  COMMENTS             PIC X(50).
       01  EMPLOYEE-STATUS       PIC X(1).
           88  ACTIVE            VALUE 'A'.
           88  INACTIVE          VALUE 'I'.
           88  SUSPENDED         VALUE 'S'.

       01  EMPLOYEE-TYPE         PIC X(1).
           88  FULL-TIME         VALUE 'F'.
           88  PART-TIME         VALUE 'P'.
           88  CONTRACTOR        VALUE 'C'.

       01  MARITAL-STATUS        PIC X(1).
           88  SINGLE            VALUE 'S'.
           88  MARRIED           VALUE 'M'.
           88  DIVORCED          VALUE 'D'.
           88  WIDOWED           VALUE 'W'.

       01  SALARY-CODE           PIC 9(3).
           88  LOW-SALARY        VALUE 001 THRU 300.
           88  MEDIUM-SALARY     VALUE 301 THRU 600.
           88  HIGH-SALARY       VALUE 601 THRU 900.
           88  EXECUTIVE-SALARY  VALUE 901 THRU 999.

       01  BONUS-ELIGIBILITY     PIC X(1).
           88  BONUS-ELIGIBLE    VALUE 'Y'.
           88  NO-BONUS          VALUE 'N'.

       01  DEPARTMENT-CODE       PIC X(3).
           88  SALES-DEPT        VALUE '001'.
           88  HR-DEPT           VALUE '002'.
           88  IT-DEPT           VALUE '003'.
           88  FINANCE-DEPT      VALUE '004'.
           88  MARKETING-DEPT    VALUE '005'.

       01  WORK-SHIFT            PIC X(1).
           88  MORNING-SHIFT     VALUE 'M'.
           88  AFTERNOON-SHIFT   VALUE 'A'.
           88  NIGHT-SHIFT       VALUE 'N'.

       01  PERFORMANCE-RATING    PIC 9(1).
           88  EXCELLENT         VALUE 5.
           88  GOOD              VALUE 4.
           88  AVERAGE           VALUE 3.
           88  BELOW-AVERAGE     VALUE 2.
           88  POOR              VALUE 1.

       01  BENEFIT-PLAN          PIC X(1).
           88  HEALTH-PLAN       VALUE 'H'.
           88  DENTAL-PLAN       VALUE 'D'.
           88  VISION-PLAN       VALUE 'V'.
           88  RETIREMENT-PLAN   VALUE 'R'.

       01  TRAVEL-ELIGIBILITY    PIC X(1).
           88  CAN-TRAVEL        VALUE 'Y'.
           88  CANNOT-TRAVEL     VALUE 'N'.

       01  PROJECT-STATUS        PIC X(1).
           88  PROJECT-NEW       VALUE 'N'.
           88  PROJECT-IN-PROGRESS VALUE 'P'.
           88  PROJECT-COMPLETED VALUE 'C'.
           88  PROJECT-CANCELLED VALUE 'X'.

       01  EMPLOYEE-LEVEL        PIC 9(1).
           88  ENTRY-LEVEL       VALUE 1.
           88  MID-LEVEL         VALUE 2.
           88  SENIOR-LEVEL      VALUE 3.
           88  EXECUTIVE-LEVEL   VALUE 4.

       01  PAY-GRADE             PIC X(1).
           88  GRADE-A           VALUE 'A'.
           88  GRADE-B           VALUE 'B'.
           88  GRADE-C           VALUE 'C'.
           88  GRADE-D           VALUE 'D'.
           88  GRADE-E           VALUE 'E'.

       01  LEAVE-STATUS          PIC X(1).
           88  ON-LEAVE          VALUE 'L'.
           88  NOT-ON-LEAVE      VALUE 'N'.

       01  TERMINATION-STATUS    PIC X(1).
           88  TERMINATED        VALUE 'T'.
           88  NOT-TERMINATED    VALUE 'N'.
       01  COMPANY-INFO.
           05  COMPANY-NAME             PIC X(30).
           05  COMPANY-ADDRESS.
               10  STREET               PIC X(30).
               10  CITY                 PIC X(20).
               10  STATE                PIC XX.
               10  ZIP                  PIC 9(5).
           05  TOTAL-EMPLOYEES          PIC 9(6).
           05  COMPANY-FOUNDED          PIC 9(4).
           05  ANNUAL-REVENUE           PIC 9(10)V99.
               
        PROCEDURE DIVISION.
           DISPLAY "Hello, World!".
           STOP RUN.