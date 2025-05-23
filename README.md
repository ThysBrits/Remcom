# REMCOM MVP Specification

## Objective
Deliver a streamlined, web-based enforcement tool for roadside officials to manually process and reconcile Warrants of Arrest (WoAs) and outstanding notices for vehicles and drivers—without ANPR, handheld devices, or mobile scanners.

---

## User Roles & Permissions

- **Officer**
  - Manual data entry (Driver ID or Vehicle Registration)
  - Lookup and view outstanding notices/WoAs
  - Mark WoA as executed
  - Mark fine/notice as paid
  - Generate and print PDFs (receipts, executed warrants)

- **Admin**
  - All Officer permissions
  - View and verify logs (payments, WoA executions)
  - Upload reconciliation XML files
  - Track and resolve mismatches

---

## Functional Requirements

### 1. Manual Data Entry Interface
- Officers can enter either a South African Driver ID or Vehicle Registration (never both).
- Input validation for both fields (SA standard formats).

### 2. Centralised WoA/Notice Lookup
- Query and display all outstanding notices or WoAs linked to the entered ID or vehicle.
- Clearly distinguish between driver-linked and vehicle-linked entries.
- Display for each record:
  - Notice/WoA number
  - Offence details
  - Date issued
  - Amount due
  - Status
  - Linked driver/vehicle info

### 3. Actionable Enforcement Interface
- Officers can:
  - Mark WoA as executed
  - Mark fine/notice as paid
- Optional comment field for each action.
- System records user ID and timestamp for each action.

### 4. Receipt and Warrant Issuance
- Generate and display printable PDFs for:
  - Payment receipts
  - Executed warrants
- PDFs must include: officer name/ID, date/time, notice/WoA details, amount paid (for receipts), and allow for logos/additional text (formatting to be finalized later).

### 5. TRAFMAN Integration (Basic Export)
- Generate a daily transaction file in XML format for manual upload into TRAFMAN.
- Include all available fields for each transaction (payments, executions, timestamps, user IDs).
- Export all transactions for the day.
- Filename must include current date and time.

### 6. Admin and Reconciliation Module
- Admins can:
  - View and verify logs of payments and WoA executions.
  - Upload reconciliation XML files.
  - System processes payments against original transgressions and marks WoAs as executed.
  - Existing transactions are skipped.
  - Filtering and sorting by all available fields in logs.

---

## Non-Functional Requirements

- **Authentication:** Role-based access (Officer, Admin). User management within the system. Standard OWASP password rules. Account lockout after 5 failed attempts.
- **Audit Logging:** Immutable logs of all events (logins, searches, payments, executions, admin actions). Only Admins can view logs.
- **Performance:** No offline support required for MVP.
- **Security:** HTTPS-only, encrypted sensitive data, OWASP top 10 compliance.
- **Deployment:** Hosted centrally (online). Standard browser support.
- **Localization:** English only, South African standard formatting. Option to add more languages later.
- **Accessibility:** Standard usability.
- **Reporting:** No additional dashboards or reports beyond specified modules.
- **Data Retention/Backup:** No special requirements for MVP.
- **Timeline:** No specific deadlines or milestones.

---

## Technology Stack

- **Backend:** .NET Core
- **Frontend:** Angular
- **Database:** PostgreSQL
- **PDF Generation:** Open-source, well-supported .NET library (e.g., [QuestPDF](https://github.com/QuestPDF/QuestPDF), [PdfSharpCore](https://github.com/ststeiger/PdfSharpCore))
- **XML Export:** Use .NET built-in XML serialization or a widely-used open-source library

---

## Additional Notes

- All requirements above are for the MVP. Future enhancements (offline support, additional languages, advanced reporting, etc.) may be added later.
- PDF and XML formats/layouts should be designed for easy future customization.

---

**End of Specification**
