CREATE DATABASE TestThesisDB2;
GO

USE TestThesisDB2;
GO

CREATE TABLE Roles (
    role_id TINYINT PRIMARY KEY,
    role_name NVARCHAR(100)
);

INSERT INTO Roles (role_id, role_name)
VALUES (1, 'user'), (2, 'admin'), (3, 'instructor'), (4, 'accountant');

CREATE TABLE CCCD (
	CCCD_id INT IDENTITY(1, 1) PRIMARY KEY,
	CCCD_number char(12),
	image_url varchar(MAX)
)

CREATE TABLE HealthCertificates (
	health_certificate_id INT IDENTITY(1, 1) PRIMARY KEY,
	image_url varchar(MAX)
)

CREATE TABLE Cities (
	city_id INT PRIMARY KEY,
	city_name NVARCHAR(100)
)

CREATE TABLE Provinces (
	province_id INT PRIMARY KEY,
	province_name NVARCHAR(100),
	city_id INT,
	fOREIGN KEY (city_id) REFERENCES Cities(city_id)
)

CREATE TABLE Wards (
	ward_id INT PRIMARY KEY,
	ward_name NVARCHAR(100),
	province_id INT,
	FOREIGN KEY (province_id) REFERENCES Provinces(province_id)
)

CREATE TABLE Addresses (
	address_id INT IDENTITY(1, 1) PRIMARY KEY,
	house_number NVARCHAR(100),
	road_name NVARCHAR(100),
	ward_id INT,
	FOREIGN KEY (ward_id) REFERENCES Wards(ward_id)
)

CREATE TABLE Users (
    [user_id] INT IDENTITY(1,1) PRIMARY KEY,
    username VARCHAR(50) UNIQUE,
    password_hash VARCHAR(255),
    email VARCHAR(100) UNIQUE,
    first_name NVARCHAR(10),
    middle_name NVARCHAR(20),
    last_name NVARCHAR(10),
    dob DATE,
    gender BIT,  -- 0 = Female, 1 = Male
    CCCD_id INT,
	health_certificate_id INT,
    phone VARCHAR(10),
    address_id INT,
	FOREIGN KEY (CCCD_id) REFERENCES CCCD(CCCD_id),
	FOREIGN KEY (health_certificate_id) REFERENCES HealthCertificates(health_certificate_id),
	FOREIGN KEY (address_id) REFERENCES Addresses(address_id)
);

CREATE TABLE UserRoles (
	user_role_id INT IDENTITY(1, 1) PRIMARY KEY,
	[user_id] INT NOT NULL,
	role_id TINYINT NOT NULL,
	FOREIGN KEY ([user_id]) REFERENCES Users([user_id]),
	FOREIGN KEY (role_id) REFERENCES Roles(role_id)
)

CREATE TABLE LicenceTypes (
    licence_type_id TINYINT PRIMARY KEY,
    licence_code VARCHAR(2) NOT NULL
);

INSERT INTO LicenceTypes (licence_type_id, licence_code)
VALUES (1, 'A1'), (2, 'A2'), (3, 'B1'), (4, 'B2'), (5, 'C'), (6, 'D'), (7, 'E');


CREATE TABLE LearningApplications (
    learning_id INT IDENTITY(1,1) PRIMARY KEY,
    learner_id INT,
    licence_type_id TINYINT,
    submitted_at DATETIME DEFAULT GETDATE(),
    learning_status TINYINT,   -- 1-pending, 2-approved, 3-rejected
    payment_status bit,
    instructor_id INT,
    assigned_at DATETIME,
    notes NVARCHAR(MAX),
    FOREIGN KEY (learner_id) REFERENCES Users([user_id]),
    FOREIGN KEY (licence_type_id) REFERENCES LicenceTypes(licence_type_id),
    FOREIGN KEY (instructor_id) REFERENCES Users([user_id])
);

CREATE TABLE InstructorSpecializations (
	is_id INT IDENTITY(1, 1) PRIMARY KEY,
    instructor_id INT NOT NULL,
    licence_type_id TINYINT NOT NULL,
    FOREIGN KEY (instructor_id) REFERENCES Users([user_id]),
    FOREIGN KEY (licence_type_id) REFERENCES LicenceTypes(licence_type_id)
);


CREATE TABLE TestApplications (
    test_id INT IDENTITY(1,1) PRIMARY KEY,
    learner_id INT,
    exam_date DATE,
    submitted_at DATETIME DEFAULT GETDATE(),
	score FLOAT,
    [status] bit,   -- 0 - failed, 1 - passed, NULL - not participated
    notes NVARCHAR(MAX),
    FOREIGN KEY (learner_id) REFERENCES LearningApplications(learning_id)
);

CREATE TABLE MockTestQuestions (
    question_id INT IDENTITY(1,1) PRIMARY KEY,
	test_type TINYINT,
    question_text NVARCHAR(MAX),
    option_a NVARCHAR(255),
    option_b NVARCHAR(255),
    option_c NVARCHAR(255),
    option_d NVARCHAR(255),
    correct_option CHAR(1),
    created_at DATETIME DEFAULT GETDATE(),
	ready_status BIT,	-- 0 = not ready, 1 = ready
	FOREIGN KEY (test_type) REFERENCES LicenceTypes(licence_type_id)
);

CREATE TABLE MockTestResults (
    result_id INT IDENTITY(1,1) PRIMARY KEY,
    [user_id] INT,
    submitted_at DATETIME DEFAULT GETDATE(),
    score INT,
    FOREIGN KEY ([user_id]) REFERENCES Users([user_id])
);

CREATE TABLE LearningMaterials (
    material_id INT IDENTITY(1,1) PRIMARY KEY,
    title NVARCHAR(100),
    [description] NVARCHAR(MAX),
    file_link NVARCHAR(255),
    created_at DATETIME DEFAULT GETDATE()
); 