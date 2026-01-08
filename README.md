***პროექტის აღწერა***

შექმენით Appointment Booking REST API, რომელიც საშუალებას იძლევა დაჯავშნოთშეხვედრები, მართოთ დროის სლოტები და გაგზავნოთ შეხსენებები. სისტემა უნდაამუშავებდეს კონფლიქტებს და recurring appointments.
```
-- service_providers
id (PK)
name
email
specialty
is_active
created_at


-- working_hours
id (PK)
provider_id (FK)
day_of_week (0-6)
start_time
end_time
is_active


-- appointments
id (PK)
provider_id (FK)
customer_name
customer_email
customer_phone
appointment_date
start_time
end_time
status (scheduled, completed, cancelled, no_show)
cancellation_reason
is_recurring
recurrence_rule
parent_appointment_id
created_at
updated_at


-- blocked_times
id (PK)
provider_id (FK)
start_datetime
end_datetime
reason
created_at


-- notification_logs
id (PK)
appointment_id (FK)
type (confirmation, reminder, cancellation)
sent_at
status
```

***ფუნქციონალი***
```
1. Service Provider Management
   • Provider profiles
   • Available time slots configuration
   • time management

2. Appointment Booking
   • Book appointment
   • Conflict detection
   • Recurring appointments (weekly/monthly)
   • Cancellation with reason
   • Rescheduling

3. Notifications
   • Email confirmation on booking
   • Reminder 24h before
   • Cancellation notifications
   • Provider notifications

4. Business Rules
   • No double booking
   • Minimum 24h advance booking
   • Maximum 3 months ahead
   • Business hours validation
   • Duration constraints (15, 30, 45, 60 min)
``` 
