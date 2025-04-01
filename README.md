# Car Rental API

## Overview
This API allows users to make car reservations for specific dates and times. The service provides a simple way to rent cars.

##  Design Decisions

### 1. **Reservation**
   - Reservations can only be made for a **specific date and time**.
   - The reservation must always be on the hour (e.g., 2:00 PM, 3:00 PM).
   - A car must be reserved at least **one hour in advance**. 
   - The **maximum rental duration is one year**. 
