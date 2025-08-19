DROP DATABASE SEP490_SU25_G90_DB;

CREATE DATABASE SEP490_SU25_G90_DB;
GO

USE SEP490_SU25_G90_DB;
GO

CREATE TABLE Roles
(
    role_id TINYINT PRIMARY KEY,
    role_name NVARCHAR(100)
);


CREATE TABLE LicenceTypes
(
    licence_type_id TINYINT PRIMARY KEY,
    licence_code VARCHAR(2) NOT NULL
);



CREATE TABLE CCCD
(
    CCCD_id INT IDENTITY PRIMARY KEY,
    CCCD_number CHAR(12) NOT NULL,
    image_mt VARCHAR(MAX),
    image_ms VARCHAR(MAX)
);

CREATE TABLE HealthCertificates
(
    health_certificate_id INT IDENTITY PRIMARY KEY,
    image_url VARCHAR(MAX)
);



-- Cities
CREATE TABLE Cities
(
    city_id INT PRIMARY KEY,
    city_name NVARCHAR(100)
);




-- Provinces
CREATE TABLE Provinces
(
    province_id INT PRIMARY KEY,
    province_name NVARCHAR(100),
    city_id INT,
    FOREIGN KEY (city_id) REFERENCES Cities(city_id)
);




-- Wards
CREATE TABLE Wards
(
    ward_id INT PRIMARY KEY,
    ward_name NVARCHAR(100),
    province_id INT,
    FOREIGN KEY (province_id) REFERENCES Provinces(province_id)
);




CREATE TABLE Addresses
(
    address_id INT IDENTITY PRIMARY KEY,
    house_number NVARCHAR(100),
    road_name NVARCHAR(100),
    ward_id INT,
    FOREIGN KEY (ward_id) REFERENCES Wards(ward_id)
);


CREATE TABLE Users
(
    user_id INT IDENTITY PRIMARY KEY,
    email VARCHAR(100),
    [password] VARCHAR(255),
    profile_image_url VARCHAR(500),
    first_name NVARCHAR(10),
    middle_name NVARCHAR(20),
    last_name NVARCHAR(10),
    dob DATE,
    gender BIT,
    -- 0 = Nu, 1 = Nam
    CCCD_id INT,
    health_certificate_id INT,
    phone VARCHAR(10),
    address_id INT,
    FOREIGN KEY (CCCD_id) REFERENCES CCCD(CCCD_id),
    FOREIGN KEY (health_certificate_id) REFERENCES HealthCertificates(health_certificate_id),
    FOREIGN KEY (address_id) REFERENCES Addresses(address_id)
);

CREATE TABLE UserRoles
(
    user_role_id INT IDENTITY PRIMARY KEY,
    user_id INT NOT NULL,
    role_id TINYINT NOT NULL,
    FOREIGN KEY (user_id) REFERENCES Users(user_id),
    FOREIGN KEY (role_id) REFERENCES Roles(role_id)
);


CREATE TABLE LearningApplications
(
    learning_id INT IDENTITY PRIMARY KEY,
    learner_id INT NOT NULL,
    licence_type_id TINYINT,
    submitted_at DATETIME DEFAULT GETDATE(),
    theory_score INT,
    simulation_score INT,
    obstacle_score INT,
    practical_score INT,
    learning_status TINYINT,
    -- 1 = dang hoc, 2 = bao luu, 3 = hoc lai, 4 = ho�n th�nh
    practical_duration_hours FLOAT, -- total hours practiced
    practical_distance FLOAT, -- kilometers
    test_eligibility BIT,
    FOREIGN KEY (learner_id) REFERENCES Users(user_id),
    FOREIGN KEY (licence_type_id) REFERENCES LicenceTypes(licence_type_id)
);

CREATE TABLE Courses (
    course_id INT IDENTITY PRIMARY KEY,
    course_name NVARCHAR(100),
    [start_date] DATE,
    end_date DATE,
    licence_type_id TINYINT,
    FOREIGN KEY (licence_type_id) REFERENCES LicenceTypes(licence_type_id)
)

CREATE TABLE Classes
(
    class_id INT IDENTITY PRIMARY KEY,
    instructor_id INT,
    class_name NVARCHAR(30),
    course_id INT,
    FOREIGN KEY (instructor_id) REFERENCES Users(user_id),
    FOREIGN KEY (course_id) REFERENCES Courses(course_id)
);

CREATE TABLE ClassMembers
(
    id INT IDENTITY PRIMARY KEY,
    class_id INT,
    learner_id INT,
    FOREIGN KEY (class_id) REFERENCES Classes(class_id),
    FOREIGN KEY (learner_id) REFERENCES LearningApplications(learning_id)
);

CREATE TABLE Attendance
(
    attendance_id INT IDENTITY PRIMARY KEY,
    learner_id INT NOT NULL,
    class_id INT NOT NULL,
    session_date DATE NOT NULL,
    attendance_status BIT, --0: absent; 1: present
    practical_duration_hours FLOAT, -- total hours practiced
    practical_distance FLOAT, -- kilometers
    note NVARCHAR(255),
    FOREIGN KEY (learner_id) REFERENCES LearningApplications(learning_id),
    FOREIGN KEY (class_id) REFERENCES Classes(class_id),
    UNIQUE (learner_id, class_id, session_date) -- avoid duplicates
);

CREATE TABLE InstructorSpecializations
(
    is_id INT IDENTITY PRIMARY KEY,
    instructor_id INT,
    licence_type_id TINYINT,
    FOREIGN KEY (instructor_id) REFERENCES Users(user_id),
    FOREIGN KEY (licence_type_id) REFERENCES LicenceTypes(licence_type_id)
);


CREATE TABLE TestApplications
(
    test_id INT IDENTITY PRIMARY KEY,
    learning_id INT,
    exam_date DATE,
    result_image_url VARCHAR(500),
    theory_score INT,
    simulation_score INT,
    obstacle_score INT,
    practical_score INT,
    [status] BIT,
    submit_profile_date DATE,
    notes NVARCHAR(MAX),
    FOREIGN KEY (learning_id) REFERENCES LearningApplications(learning_id),
);


CREATE TABLE LearningMaterials
(
    material_id INT IDENTITY PRIMARY KEY,
    title NVARCHAR(100),
    [description] NVARCHAR(MAX),
    licence_type_id TINYINT,
    file_link NVARCHAR(MAX),
    created_at DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (licence_type_id) REFERENCES LicenceTypes(licence_type_id)
);


CREATE TABLE News
(
    news_id INT IDENTITY PRIMARY KEY,
    title NVARCHAR(500),
    news_content NVARCHAR(MAX),
    author_id INT,
    post_time DATETIME,
    image VARCHAR(250),
    FOREIGN KEY (author_id) REFERENCES Users(user_id)
);


CREATE TABLE TestScoreStandards
(
    id INT IDENTITY PRIMARY KEY,
    licence_type_id TINYINT,
    part_name NVARCHAR(50),
    max_score INT,
    pass_score INT,
    FOREIGN KEY (licence_type_id) REFERENCES LicenceTypes(licence_type_id)
);

CREATE TABLE ScheduleSlots
(
    slot_id INT PRIMARY KEY,
    start_time TIME,
    end_time TIME
)

CREATE TABLE ClassSchedules
(
    schedule_id INT IDENTITY PRIMARY KEY,
    class_id INT,
    slot_id INT,
    schedule_date DATE,
    FOREIGN KEY (class_id) REFERENCES Classes(class_id),
    FOREIGN KEY (slot_id) REFERENCES ScheduleSlots(slot_id)
);

CREATE TABLE Cars
(
    car_id INT IDENTITY PRIMARY KEY,
    license_plate NVARCHAR(13),
    car_make NVARCHAR(50),
    car_model NVARCHAR(50)
)

CREATE TABLE CarAssignmentStatus
(
    status_id TINYINT PRIMARY KEY,
    status_name NVARCHAR(50)
);

CREATE TABLE CarAssignments
(
    assignment_id INT IDENTITY PRIMARY KEY,
    car_id INT NOT NULL,
    instructor_id INT NOT NULL,
    slot_id INT NOT NULL,
    schedule_date DATE,
    car_status TINYINT,
    FOREIGN KEY (car_id) REFERENCES Cars(car_id),
    FOREIGN KEY (instructor_id) REFERENCES Users(user_id),
    FOREIGN KEY (slot_id) REFERENCES ScheduleSlots(slot_id),
    FOREIGN KEY (car_status) REFERENCES CarAssignmentStatus(status_id)
);

