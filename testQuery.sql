DROP DATABASE SEP490_SU25_G90_DB;

CREATE DATABASE SEP490_SU25_G90_DB;
GO

USE SEP490_SU25_G90_DB;
GO

CREATE TABLE Roles (
    role_id TINYINT PRIMARY KEY,
    role_name NVARCHAR(100)
);
INSERT INTO Roles VALUES 
(1, 'learner'), (2, 'admin'), (3, 'instructor');

CREATE TABLE LicenceTypes (
    licence_type_id TINYINT PRIMARY KEY,
    licence_code VARCHAR(2) NOT NULL
);
INSERT INTO LicenceTypes VALUES 
(1, 'B1'), (2, 'B2'), (3, 'C'), (4, 'D'), (5, 'E');


CREATE TABLE CCCD (
    CCCD_id INT IDENTITY PRIMARY KEY,
    CCCD_number CHAR(12) NOT NULL,
    image_mt VARCHAR(MAX),
	image_ms VARCHAR(MAX)
);

CREATE TABLE HealthCertificates (
    health_certificate_id INT IDENTITY PRIMARY KEY,
    image_url VARCHAR(MAX)
);

CREATE TABLE Cities (
    city_id INT PRIMARY KEY,
    city_name NVARCHAR(100)
);

CREATE TABLE Provinces (
    province_id INT PRIMARY KEY,
    province_name NVARCHAR(100),
    city_id INT,
    FOREIGN KEY (city_id) REFERENCES Cities(city_id)
);

CREATE TABLE Wards (
    ward_id INT PRIMARY KEY,
    ward_name NVARCHAR(100),
    province_id INT,
    FOREIGN KEY (province_id) REFERENCES Provinces(province_id)
);

CREATE TABLE Addresses (
    address_id INT IDENTITY PRIMARY KEY,
    house_number NVARCHAR(100),
    road_name NVARCHAR(100),
    ward_id INT,
    FOREIGN KEY (ward_id) REFERENCES Wards(ward_id)
);


CREATE TABLE Users (
    user_id INT IDENTITY PRIMARY KEY,
    email VARCHAR(100),
    [password] VARCHAR(255),
    profile_image_url VARCHAR(500),
    first_name NVARCHAR(10),
    middle_name NVARCHAR(20),
    last_name NVARCHAR(10),
    dob DATE,
    gender BIT,  -- 0 = Nu, 1 = Nam
    CCCD_id INT,
    health_certificate_id INT,
    phone VARCHAR(10),
    address_id INT,
    FOREIGN KEY (CCCD_id) REFERENCES CCCD(CCCD_id),
    FOREIGN KEY (health_certificate_id) REFERENCES HealthCertificates(health_certificate_id),
    FOREIGN KEY (address_id) REFERENCES Addresses(address_id)
);

CREATE TABLE UserRoles (
    user_role_id INT IDENTITY PRIMARY KEY,
    user_id INT NOT NULL,
    role_id TINYINT NOT NULL,
    FOREIGN KEY (user_id) REFERENCES Users(user_id),
    FOREIGN KEY (role_id) REFERENCES Roles(role_id)
);


CREATE TABLE LearningApplications (
    learning_id INT IDENTITY PRIMARY KEY,
    learner_id INT NOT NULL,
    licence_type_id TINYINT,
    submitted_at DATETIME DEFAULT GETDATE(),
    theory_score INT,
    simulation_score INT,
    obstacle_score INT,
    practical_score INT,
    learning_status TINYINT, -- 1 = dang hoc, 2 = bao luu, 3 = hoc lai, 4 = ho�n th�nh
    test_eligibility BIT,
    FOREIGN KEY (learner_id) REFERENCES Users(user_id),
    FOREIGN KEY (licence_type_id) REFERENCES LicenceTypes(licence_type_id)
);


CREATE TABLE Classes (
    class_id INT IDENTITY PRIMARY KEY,
    instructor_id INT,
    licence_type_id TINYINT,
	class_name NVARCHAR(30),
    FOREIGN KEY (instructor_id) REFERENCES Users(user_id),
    FOREIGN KEY (licence_type_id) REFERENCES LicenceTypes(licence_type_id)
);

CREATE TABLE ClassMembers (
    id INT IDENTITY PRIMARY KEY,
    class_id INT,
    learner_id INT,
    FOREIGN KEY (class_id) REFERENCES Classes(class_id),
    FOREIGN KEY (learner_id) REFERENCES Users(user_id)
);


CREATE TABLE InstructorSpecializations (
    is_id INT IDENTITY PRIMARY KEY,
    instructor_id INT,
    licence_type_id TINYINT,
    FOREIGN KEY (instructor_id) REFERENCES Users(user_id),
    FOREIGN KEY (licence_type_id) REFERENCES LicenceTypes(licence_type_id)
);


CREATE TABLE TestApplications (
    test_id INT IDENTITY PRIMARY KEY,
    learning_id INT,
    exam_date DATE,
    result_image_url VARCHAR(500),
    theory_score INT,
    simulation_score INT,
    obstacle_score INT,
    practical_score INT,
    [status] BIT,
    created_at DATETIME,
    notes NVARCHAR(MAX),
    FOREIGN KEY (learning_id) REFERENCES LearningApplications(learning_id),
);


CREATE TABLE LearningMaterials (
    material_id INT IDENTITY PRIMARY KEY,
    title NVARCHAR(100),
    [description] NVARCHAR(MAX),
    licence_type_id TINYINT,
    file_link NVARCHAR(MAX),
    created_at DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (licence_type_id) REFERENCES LicenceTypes(licence_type_id)
);


CREATE TABLE News (
    news_id INT IDENTITY PRIMARY KEY,
    title NVARCHAR(500),
    news_content NVARCHAR(MAX),
    author_id INT,
    post_time DATETIME,
    image VARCHAR(250),
    FOREIGN KEY (author_id) REFERENCES Users(user_id)
);


CREATE TABLE TestScoreStandards (
    id INT IDENTITY PRIMARY KEY,
    licence_type_id TINYINT,
    part_name NVARCHAR(50),
    max_score INT,
    pass_score INT,
    FOREIGN KEY (licence_type_id) REFERENCES LicenceTypes(licence_type_id)
);

USE SEP490_SU25_G90_DB;
GO

-- Insert sample users with proper Vietnamese Unicode characters
INSERT INTO Users (email, [password], first_name, middle_name, last_name, dob, gender, phone)
VALUES 
-- Learners (Học viên)
(N'nguyen.thi.mai@example.com', 'hashed_password_1', N'Nguyễn', N'Thị', N'Mai', '2000-05-15', 0, '0912345678'),
(N'tran.van.tuan@example.com', 'hashed_password_2', N'Trần', N'Văn', N'Tuấn', '1999-08-22', 1, '0923456789'),
(N'le.thi.lan@example.com', 'hashed_password_3', N'Lê', N'Thị', N'Lan', '2001-03-10', 0, '0934567890'),

-- Admins (Quản trị viên)
(N'pham.duc.hai@example.com', 'hashed_admin_1', N'Phạm', N'Đức', N'Hải', '1985-11-30', 1, '0945678901'),
(N'vu.quang.minh@example.com', 'hashed_admin_2', N'Vũ', N'Quang', N'Minh', '1988-07-25', 1, '0956789012'),
(N'do.thanh.thuy@example.com', 'hashed_admin_3', N'Đỗ', N'Thanh', N'Thủy', '1990-04-18', 0, '0967890123'),

-- Instructors (Giáo viên)
(N'hoang.manh.hung@example.com', 'hashed_instructor_1', N'Hoàng', N'Mạnh', N'Hùng', '1980-09-12', 1, '0978901234'),
(N'truong.thi.linh@example.com', 'hashed_instructor_2', N'Trương', N'Thị', N'Linh', '1983-06-05', 0, '0989012345'),
(N'nguyen.ba.son@example.com', 'hashed_instructor_3', N'Nguyễn', N'Bá', N'Sơn', '1978-12-20', 1, '0990123456');
GO

-- Assign roles to users
INSERT INTO UserRoles (user_id, role_id)
VALUES 
-- Assign learner role (1) to first 3 users
(1, 1),
(2, 1),
(3, 1),

-- Assign admin role (2) to next 3 users
(4, 2),
(5, 2),
(6, 2),

-- Assign instructor role (3) to last 3 users
(7, 3),
(8, 3),
(9, 3);
GO

-- Verify the inserted data with proper Unicode display
SELECT 
    u.user_id,
    CONCAT(u.last_name, ' ', u.middle_name, ' ', u.first_name) AS full_name,
    u.email,
    u.phone,
    CASE u.gender WHEN 1 THEN N'Nam' ELSE N'Nữ' END AS gender,
    r.role_name AS role
FROM Users u
JOIN UserRoles ur ON u.user_id = ur.user_id
JOIN Roles r ON ur.role_id = r.role_id
ORDER BY u.user_id;
GO