# 🍔 LoveAtFirstBite

**Group Project: Lunch Voting App (Team Edition)**  
A web application that allows users to vote once per day on where the team should go for lunch.  
All users share the same pool of restaurants, and the app resets votes automatically.

---

## 🧱 Entities

- **User**
  - Id
  - UserName
  - UserEmail
  - Password (hashed)

- **Restaurant**
  - Id
  - Name
  - Address
  - CreatedByUserId (FK to User)

- **Vote**
  - Id
  - RestaurantId (FK)
  - UserId (FK)
  - Date

---

## ✅ Functional Requirements

- Register/login as a user
- Add new restaurants to the shared list
- Cast one vote per user per day
- Display current vote results
- Change vote
- Automatic reset at midnight

---

## 🧪 Non-Functional Requirements

- Secure login/authentication
- API response time < 500ms
- Support at least 50 users
- Responsive web design

---

## 🧭 MoSCoW Prioritization

- **Must Have**  
  - Login/register user  
  - Add restaurant  
  - One vote per user per day  
  - Reset votes daily

- **Should Have**  
  - Change vote

- **Could Have**  
  - Change vote shortcut  
  - Forgot password reset  
  - Users can change automatic reset time

- **Won’t Have (for now)**  
  - Multiple teams  
  - Admin dashboard  
  - Notifications/SMS

---

## 👤 User Stories

- As a user, I want to register and log in so I can participate in voting.
- As a user, I want to add a restaurant to the shared list.
- As a user, I want to vote once per day so my vote counts toward the lunch decision.
- As a user, I want to see which restaurant is winning today.
- As a user, I want to be able to change my vote.
- As a user, I want the vote count to reset daily.
- As a user, I want to be able to change the reset time.

---

## 📋 Use Cases

### 1. Register and Log In

- **Actor**: User  
- **Description**: A user creates an account or logs in to access voting features  
- **Preconditions**: User is not currently logged in  
- **Postconditions**: User is authenticated and session/token is stored  

**Flow**:
1. User navigates to login/register page  
2. User enters credentials  
3. System validates input and logs user in (or creates account)  
4. User is redirected to the main dashboard  

---

### 2. Add a Restaurant

- **Actor**: Authenticated User  
- **Description**: A logged-in user submits a new restaurant to the shared pool  
- **Preconditions**: User is logged in  
- **Postconditions**: Restaurant is saved and visible to all users  

**Flow**:
1. User opens “Add Restaurant” form  
2. User enters name and (optional) description  
3. System saves the restaurant with the user’s ID as `AddedByUserId`  

---

### 3. Cast a Vote

- **Actor**: Authenticated User  
- **Description**: A user votes once per day for a restaurant  
- **Preconditions**: User is logged in and hasn’t voted today  
- **Postconditions**: Vote is saved and linked to user + restaurant  

**Flow**:
1. User clicks “Vote” on a restaurant  
2. System checks if the user already voted today  
3. If not, system saves the vote and updates live count  

**Alternative Flow**:
- If user already voted → system shows error or lets user change vote

---

### 4. View Today’s Top Restaurant

- **Actor**: Any User  
- **Description**: User sees the restaurant with the most votes for today  
- **Preconditions**: At least one restaurant and one vote exist  
- **Postconditions**: UI displays vote results  

**Flow**:
1. User opens vote summary or homepage  
2. System fetches current vote tally  
3. Highest-voted restaurant is shown as the leader  

---

### 5. Change My Vote

- **Actor**: Authenticated User  
- **Description**: A user can change their vote before reset  
- **Preconditions**: User is logged in and has already voted today  
- **Postconditions**: New vote is saved, old one is removed  

**Flow**:
1. User clicks “Change Vote”  
2. System deletes today’s existing vote  
3. User selects a new restaurant  
4. System saves the new vote  

---

### 6. Daily Reset

- **Actor**: System (automated)  
- **Description**: System clears all votes at a scheduled time each day  
- **Preconditions**: Time equals configured reset time  
- **Postconditions**: Vote table is cleared  

**Flow**:
1. Background task runs at scheduled time (e.g., midnight)  
2. System deletes all `Vote` entries  
3. Leaderboard resets  

---

### 7. Change Vote Reset Time

- **Actor**: Admin  
- **Description**: Admin configures the daily reset time  
- **Preconditions**: Admin is logged in  
- **Postconditions**: New reset time is saved and used for next day  

**Flow**:
1. Admin opens system settings  
2. Selects new reset time  
3. System updates scheduled task
