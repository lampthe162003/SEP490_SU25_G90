USE SEP490_SU25_G90_DB;
GO

INSERT INTO Roles VALUES 
(1, 'learner'), (2, 'admin'), (3, 'instructor');

INSERT INTO LicenceTypes VALUES 
(1, 'B1'), (2, 'B2'), (3, 'C'), (4, 'D'), (5, 'E');

INSERT [dbo].[ScheduleSlots]
VALUES (1, '08:00:00', '09:30:00'), (2, '10:00:00', '11:30:00'), (3, '13:30:00', '15:00:00'), (4, '15:30:00', '17:00:00')
-- Insert TestCoreStandards
--  B1	
INSERT INTO TestScoreStandards (licence_type_id, part_name, max_score, pass_score) VALUES
(1, N'Theory', 30, 26),
(1, N'Simulation', 50, 35),
(1, N'Obstacle', 100, 80),
(1, N'Practical', 100, 80);

--  B2
INSERT INTO TestScoreStandards (licence_type_id, part_name, max_score, pass_score) VALUES
(2, N'Theory', 35, 32),
(2, N'Simulation', 50, 35),
(2, N'Obstacle', 100, 80),
(2, N'Practical', 100, 80);

--  C
INSERT INTO TestScoreStandards (licence_type_id, part_name, max_score, pass_score) VALUES
(3, N'Theory', 40, 36),
(3, N'Simulation', 50, 35),
(3, N'Obstacle', 100, 80),
(3, N'Practical', 100, 80);

--  D
INSERT INTO TestScoreStandards (licence_type_id, part_name, max_score, pass_score) VALUES
(4, N'Theory', 45, 40),
(4, N'Simulation', 50, 35),
(4, N'Obstacle', 100, 80),
(4, N'Practical', 100, 80);

--  E
INSERT INTO TestScoreStandards (licence_type_id, part_name, max_score, pass_score) VALUES
(5, N'Theory', 45, 40),
(5, N'Simulation', 50, 35),
(5, N'Obstacle', 100, 80),
(5, N'Practical', 100, 80);

INSERT INTO Cities(city_id, city_name) VALUES
(01, N'Hà Nội'),
(02, N'Hà Giang'),
(04, N'Cao Bằng'),
(06, N'Bắc Kạn'),
(08, N'Tuyên Quang'),
(10, N'Lào Cai'),
(11, N'Điện Biên'),
(12, N'Lai Châu'),
(14, N'Sơn La'),
(15, N'Yên Bái'),
(17, N'Hoà Bình'),
(19, N'Thái Nguyên'),
(20, N'Lạng Sơn'),
(22, N'Quảng Ninh'),
(24, N'Bắc Giang'),
(25, N'Phú Thọ'),
(26, N'Vĩnh Phúc'),
(27, N'Bắc Ninh'),
(30, N'Hải Dương'),
(31, N'Hải Phòng'),
(33, N'Hưng Yên'),
(34, N'Thái Bình'),
(35, N'Hà Nam'),
(36, N'Nam Định'),
(37, N'Ninh Bình'),
(38, N'Thanh Hóa'),
(40, N'Nghệ An'),
(42, N'Hà Tĩnh'),
(44, N'Quảng Bình'),
(45, N'Quảng Trị'),
(46, N'Huế'),
(48, N'Đà Nẵng'),
(49, N'Quảng Nam'),
(51, N'Quảng Ngãi'),
(52, N'Bình Định'),
(54, N'Phú Yên'),
(56, N'Khánh Hòa'),
(58, N'Ninh Thuận'),
(60, N'Bình Thuận'),
(62, N'Kon Tum'),
(64, N'Gia Lai'),
(66, N'Đắk Lắk'),
(67, N'Đắk Nông'),
(68, N'Lâm Đồng'),
(70, N'Bình Phước'),
(72, N'Tây Ninh'),
(74, N'Bình Dương'),
(75, N'Đồng Nai'),
(77, N'Bà Rịa - Vũng Tàu'),
(79, N'Hồ Chí Minh'),
(80, N'Long An'),
(82, N'Tiền Giang'),
(83, N'Bến Tre'),
(84, N'Trà Vinh'),
(86, N'Vĩnh Long'),
(87, N'Đồng Tháp'),
(89, N'An Giang'),
(91, N'Kiên Giang'),
(92, N'Cần Thơ'),
(93, N'Hậu Giang'),
(94, N'Sóc Trăng'),
(95, N'Bạc Liêu'),
(96, N'Cà Mau');  -- Truncated to avoid SQL size overflow

INSERT INTO Provinces(province_id, province_name, city_id) VALUES
(001, N'Ba Đình', 01),
(002, N'Hoàn Kiếm', 01),
(003, N'Tây Hồ', 01),
(004, N'Long Biên', 01),
(005, N'Cầu Giấy', 01),
(006, N'Đống Đa', 01),
(007, N'Hai Bà Trưng', 01),
(008, N'Hoàng Mai', 01),
(009, N'Thanh Xuân', 01),
(016, N'Sóc Sơn', 01),
(017, N'Đông Anh', 01),
(018, N'Gia Lâm', 01),
(019, N'Nam Từ Liêm', 01),
(020, N'Thanh Trì', 01),
(021, N'Bắc Từ Liêm', 01),
(250, N'Mê Linh', 01),
(268, N'Hà Đông', 01),
(269, N'Sơn Tây', 01),
(271, N'Ba Vì', 01),
(272, N'Phúc Thọ', 01),
(273, N'Đan Phượng', 01),
(274, N'Hoài Đức', 01),
(275, N'Quốc Oai', 01),
(276, N'Thạch Thất', 01),
(277, N'Chương Mỹ', 01),
(278, N'Thanh Oai', 01),
(279, N'Thường Tín', 01),
(280, N'Phú Xuyên', 01),
(281, N'Ứng Hòa', 01),
(282, N'Mỹ Đức', 01),
(024, N'Hà Giang', 02),
(026, N'Đồng Văn', 02),
(027, N'Mèo Vạc', 02),
(028, N'Yên Minh', 02),
(029, N'Quản Bạ', 02),
(030, N'Vị Xuyên', 02),
(031, N'Bắc Mê', 02),
(032, N'Hoàng Su Phì', 02),
(033, N'Xín Mần', 02),
(034, N'Bắc Quang', 02),
(035, N'Quang Bình', 02),
(040, N'Cao Bằng', 04),
(042, N'Bảo Lâm', 04),
(043, N'Bảo Lạc', 04),
(045, N'Hà Quảng', 04),
(047, N'Trùng Khánh', 04),
(048, N'Hạ Lang', 04),
(049, N'Quảng Hòa', 04),
(051, N'Hoà An', 04),
(052, N'Nguyên Bình', 04),
(053, N'Thạch An', 04),
(058, N'Bắc Kạn', 06),
(060, N'Pác Nặm', 06),
(061, N'Ba Bể', 06),
(062, N'Ngân Sơn', 06),
(063, N'Bạch Thông', 06),
(064, N'Chợ Đồn', 06),
(065, N'Chợ Mới', 06),
(066, N'Na Rì', 06),
(070, N'Tuyên Quang', 08),
(071, N'Lâm Bình', 08),
(072, N'Na Hang', 08),
(073, N'Chiêm Hóa', 08),
(074, N'Hàm Yên', 08),
(075, N'Yên Sơn', 08),
(076, N'Sơn Dương', 08),
(080, N'Lào Cai', 10),
(082, N'Bát Xát', 10),
(083, N'Mường Khương', 10),
(084, N'Si Ma Cai', 10),
(085, N'Bắc Hà', 10),
(086, N'Bảo Thắng', 10),
(087, N'Bảo Yên', 10),
(088, N'Sa Pa', 10),
(089, N'Văn Bàn', 10),
(094, N'Điện Biên Phủ', 11),
(095, N'Mường Lay', 11),
(096, N'Mường Nhé', 11),
(097, N'Mường Chà', 11),
(098, N'Tủa Chùa', 11),
(099, N'Tuần Giáo', 11),
(100, N'Điện Biên', 11),
(101, N'Điện Biên Đông', 11),
(102, N'Mường Ảng', 11),
(103, N'Nậm Pồ', 11),
(105, N'Lai Châu', 12),
(106, N'Tam Đường', 12),
(107, N'Mường Tè', 12),
(108, N'Sìn Hồ', 12),
(109, N'Phong Thổ', 12),
(110, N'Than Uyên', 12),
(111, N'Tân Uyên', 12),
(112, N'Nậm Nhùn', 12),
(116, N'Sơn La', 14),
(118, N'Quỳnh Nhai', 14),
(119, N'Thuận Châu', 14),
(120, N'Mường La', 14),
(121, N'Bắc Yên', 14),
(122, N'Phù Yên', 14),
(123, N'Mộc Châu', 14),
(124, N'Yên Châu', 14),
(125, N'Mai Sơn', 14),
(126, N'Sông Mã', 14),
(127, N'Sốp Cộp', 14),
(128, N'Vân Hồ', 14),
(132, N'Yên Bái', 15),
(133, N'Nghĩa Lộ', 15),
(135, N'Lục Yên', 15),
(136, N'Văn Yên', 15),
(137, N'Mù Căng Chải', 15),
(138, N'Trấn Yên', 15),
(139, N'Trạm Tấu', 15),
(140, N'Văn Chấn', 15),
(141, N'Yên Bình', 15),
(148, N'Hòa Bình', 17),
(150, N'Đà Bắc', 17),
(152, N'Lương Sơn', 17),
(153, N'Kim Bôi', 17),
(154, N'Cao Phong', 17),
(155, N'Tân Lạc', 17),
(156, N'Mai Châu', 17),
(157, N'Lạc Sơn', 17),
(158, N'Yên Thủy', 17),
(159, N'Lạc Thủy', 17),
(164, N'Thái Nguyên', 19),
(165, N'Sông Công', 19),
(167, N'Định Hóa', 19),
(168, N'Phú Lương', 19),
(169, N'Đồng Hỷ', 19),
(170, N'Võ Nhai', 19),
(171, N'Đại Từ', 19),
(172, N'Phổ Yên', 19),
(173, N'Phú Bình', 19),
(178, N'Lạng Sơn', 20),
(180, N'Tràng Định', 20),
(181, N'Bình Gia', 20),
(182, N'Văn Lãng', 20),
(183, N'Cao Lộc', 20),
(184, N'Văn Quan', 20),
(185, N'Bắc Sơn', 20),
(186, N'Hữu Lũng', 20),
(187, N'Chi Lăng', 20),
(188, N'Lộc Bình', 20),
(189, N'Đình Lập', 20),
(193, N'Hạ Long', 22),
(194, N'Móng Cái', 22),
(195, N'Cẩm Phả', 22),
(196, N'Uông Bí', 22),
(198, N'Bình Liêu', 22),
(199, N'Tiên Yên', 22),
(200, N'Đầm Hà', 22),
(201, N'Hải Hà', 22),
(202, N'Ba Chẽ', 22),
(203, N'Vân Đồn', 22),
(205, N'Đông Triều', 22),
(206, N'Quảng Yên', 22),
(207, N'Cô Tô', 22),
(213, N'Bắc Giang', 24),
(215, N'Yên Thế', 24),
(216, N'Tân Yên', 24),
(217, N'Lạng Giang', 24),
(218, N'Lục Nam', 24),
(219, N'Lục Ngạn', 24),
(220, N'Sơn Động', 24),
(222, N'Việt Yên', 24),
(223, N'Hiệp Hòa', 24),
(224, N'Chũ', 24),
(227, N'Việt Trì', 25),
(228, N'Phú Thọ', 25),
(230, N'Đoan Hùng', 25),
(231, N'Hạ Hoà', 25),
(232, N'Thanh Ba', 25),
(233, N'Phù Ninh', 25),
(234, N'Yên Lập', 25),
(235, N'Cẩm Khê', 25),
(236, N'Tam Nông', 25),
(237, N'Lâm Thao', 25),
(238, N'Thanh Sơn', 25),
(239, N'Thanh Thuỷ', 25),
(240, N'Tân Sơn', 25),
(243, N'Vĩnh Yên', 26),
(244, N'Phúc Yên', 26),
(246, N'Lập Thạch', 26),
(247, N'Tam Dương', 26),
(248, N'Tam Đảo', 26),
(249, N'Bình Xuyên', 26),
(251, N'Yên Lạc', 26),
(252, N'Vĩnh Tường', 26),
(253, N'Sông Lô', 26),
(256, N'Bắc Ninh', 27),
(258, N'Yên Phong', 27),
(259, N'Quế Võ', 27),
(260, N'Tiên Du', 27),
(261, N'Từ Sơn', 27),
(262, N'Thuận Thành', 27),
(263, N'Gia Bình', 27),
(264, N'Lương Tài', 27),
(288, N'Hải Dương', 30),
(290, N'Chí Linh', 30),
(291, N'Nam Sách', 30),
(292, N'Kinh Môn', 30),
(293, N'Kim Thành', 30),
(294, N'Thanh Hà', 30),
(295, N'Cẩm Giàng', 30),
(296, N'Bình Giang', 30),
(297, N'Gia Lộc', 30),
(298, N'Tứ Kỳ', 30),
(299, N'Ninh Giang', 30),
(300, N'Thanh Miện', 30),
(303, N'Hồng Bàng', 31),
(304, N'Ngô Quyền', 31),
(305, N'Lê Chân', 31),
(306, N'Hải An', 31),
(307, N'Kiến An', 31),
(308, N'Đồ Sơn', 31),
(309, N'Dương Kinh', 31),
(311, N'Thuỷ Nguyên', 31),
(312, N'An Dương', 31),
(313, N'An Lão', 31),
(314, N'Kiến Thuỵ', 31),
(315, N'Tiên Lãng', 31),
(316, N'Vĩnh Bảo', 31),
(317, N'Cát Hải', 31),
(318, N'Bạch Long Vĩ', 31),
(323, N'Hưng Yên', 33),
(325, N'Văn Lâm', 33),
(326, N'Văn Giang', 33),
(327, N'Yên Mỹ', 33),
(328, N'Mỹ Hào', 33),
(329, N'Ân Thi', 33),
(330, N'Khoái Châu', 33),
(331, N'Kim Động', 33),
(332, N'Tiên Lữ', 33),
(333, N'Phù Cừ', 33),
(336, N'Thái Bình', 34),
(338, N'Quỳnh Phụ', 34),
(339, N'Hưng Hà', 34),
(340, N'Đông Hưng', 34),
(341, N'Thái Thụy', 34),
(342, N'Tiền Hải', 34),
(343, N'Kiến Xương', 34),
(344, N'Vũ Thư', 34),
(347, N'Phủ Lý', 35),
(349, N'Duy Tiên', 35),
(350, N'Kim Bảng', 35),
(351, N'Thanh Liêm', 35),
(352, N'Bình Lục', 35),
(353, N'Lý Nhân', 35),
(356, N'Nam Định', 36),
(359, N'Vụ Bản', 36),
(360, N'Ý Yên', 36),
(361, N'Nghĩa Hưng', 36),
(362, N'Nam Trực', 36),
(363, N'Trực Ninh', 36),
(364, N'Xuân Trường', 36),
(365, N'Giao Thủy', 36),
(366, N'Hải Hậu', 36),
(370, N'Tam Điệp', 37),
(372, N'Nho Quan', 37),
(373, N'Gia Viễn', 37),
(374, N'Hoa Lư', 37),
(375, N'Yên Khánh', 37),
(376, N'Kim Sơn', 37),
(377, N'Yên Mô', 37),
(380, N'Thanh Hóa', 38),
(381, N'Bỉm Sơn', 38),
(382, N'Sầm Sơn', 38),
(384, N'Mường Lát', 38),
(385, N'Quan Hóa', 38),
(386, N'Bá Thước', 38),
(387, N'Quan Sơn', 38),
(388, N'Lang Chánh', 38),
(389, N'Ngọc Lặc', 38),
(390, N'Cẩm Thủy', 38),
(391, N'Thạch Thành', 38),
(392, N'Hà Trung', 38),
(393, N'Vĩnh Lộc', 38),
(394, N'Yên Định', 38),
(395, N'Thọ Xuân', 38),
(396, N'Thường Xuân', 38),
(397, N'Triệu Sơn', 38),
(398, N'Thiệu Hóa', 38),
(399, N'Hoằng Hóa', 38),
(400, N'Hậu Lộc', 38),
(401, N'Nga Sơn', 38),
(402, N'Như Xuân', 38),
(403, N'Như Thanh', 38),
(404, N'Nông Cống', 38),
(406, N'Quảng Xương', 38),
(407, N'Nghi Sơn', 38),
(412, N'Vinh', 40),
(414, N'Thái Hoà', 40),
(415, N'Quế Phong', 40),
(416, N'Quỳ Châu', 40),
(417, N'Kỳ Sơn', 40),
(418, N'Tương Dương', 40),
(419, N'Nghĩa Đàn', 40),
(420, N'Quỳ Hợp', 40),
(421, N'Quỳnh Lưu', 40),
(422, N'Con Cuông', 40),
(423, N'Tân Kỳ', 40),
(424, N'Anh Sơn', 40),
(425, N'Diễn Châu', 40),
(426, N'Yên Thành', 40),
(427, N'Đô Lương', 40),
(428, N'Thanh Chương', 40),
(429, N'Nghi Lộc', 40),
(430, N'Nam Đàn', 40),
(431, N'Hưng Nguyên', 40),
(432, N'Hoàng Mai', 40),
(436, N'Hà Tĩnh', 42),
(437, N'Hồng Lĩnh', 42),
(439, N'Hương Sơn', 42),
(440, N'Đức Thọ', 42),
(441, N'Vũ Quang', 42),
(442, N'Nghi Xuân', 42),
(443, N'Can Lộc', 42),
(444, N'Hương Khê', 42),
(445, N'Thạch Hà', 42),
(446, N'Cẩm Xuyên', 42),
(447, N'Kỳ Anh', 42),
(449, N'Kỳ Anh', 42),
(450, N'Đồng Hới', 44),
(452, N'Minh Hóa', 44),
(453, N'Tuyên Hóa', 44),
(454, N'Quảng Trạch', 44),
(455, N'Bố Trạch', 44),
(456, N'Quảng Ninh', 44),
(457, N'Lệ Thủy', 44),
(458, N'Ba Đồn', 44),
(461, N'Đông Hà', 45),
(462, N'Quảng Trị', 45),
(464, N'Vĩnh Linh', 45),
(465, N'Hướng Hóa', 45),
(466, N'Gio Linh', 45),
(467, N'Đa Krông', 45),
(468, N'Cam Lộ', 45),
(469, N'Triệu Phong', 45),
(470, N'Hải Lăng', 45),
(471, N'Cồn Cỏ', 45),
(474, N'Thuận Hóa', 46),
(475, N'Phú Xuân', 46),
(476, N'Phong Điền', 46),
(477, N'Quảng Điền', 46),
(478, N'Phú Vang', 46),
(479, N'Hương Thủy', 46),
(480, N'Hương Trà', 46),
(481, N'A Lưới', 46),
(482, N'Phú Lộc', 46),
(490, N'Liên Chiểu', 48),
(491, N'Thanh Khê', 48),
(492, N'Hải Châu', 48),
(493, N'Sơn Trà', 48),
(494, N'Ngũ Hành Sơn', 48),
(495, N'Cẩm Lệ', 48),
(497, N'Hòa Vang', 48),
(498, N'Hoàng Sa', 48),
(502, N'Tam Kỳ', 49),
(503, N'Hội An', 49),
(504, N'Tây Giang', 49),
(505, N'Đông Giang', 49),
(506, N'Đại Lộc', 49),
(507, N'Điện Bàn', 49),
(508, N'Duy Xuyên', 49),
(509, N'Quế Sơn', 49),
(510, N'Nam Giang', 49),
(511, N'Phước Sơn', 49),
(512, N'Hiệp Đức', 49),
(513, N'Thăng Bình', 49),
(514, N'Tiên Phước', 49),
(515, N'Bắc Trà My', 49),
(516, N'Nam Trà My', 49),
(517, N'Núi Thành', 49),
(518, N'Phú Ninh', 49),
(522, N'Quảng Ngãi', 51),
(524, N'Bình Sơn', 51),
(525, N'Trà Bồng', 51),
(527, N'Sơn Tịnh', 51),
(528, N'Tư Nghĩa', 51),
(529, N'Sơn Hà', 51),
(530, N'Sơn Tây', 51),
(531, N'Minh Long', 51),
(532, N'Nghĩa Hành', 51),
(533, N'Mộ Đức', 51),
(534, N'Đức Phổ', 51),
(535, N'Ba Tơ', 51),
(536, N'Lý Sơn', 51),
(540, N'Quy Nhơn', 52),
(542, N'An Lão', 52),
(543, N'Hoài Nhơn', 52),
(544, N'Hoài Ân', 52),
(545, N'Phù Mỹ', 52),
(546, N'Vĩnh Thạnh', 52),
(547, N'Tây Sơn', 52),
(548, N'Phù Cát', 52),
(549, N'An Nhơn', 52),
(550, N'Tuy Phước', 52),
(551, N'Vân Canh', 52),
(555, N'Tuy Hoà', 54),
(557, N'Sông Cầu', 54),
(558, N'Đồng Xuân', 54),
(559, N'Tuy An', 54),
(560, N'Sơn Hòa', 54),
(561, N'Sông Hinh', 54),
(562, N'Tây Hoà', 54),
(563, N'Phú Hoà', 54),
(564, N'Đông Hòa', 54),
(568, N'Nha Trang', 56),
(569, N'Cam Ranh', 56),
(570, N'Cam Lâm', 56),
(571, N'Vạn Ninh', 56),
(572, N'Ninh Hòa', 56),
(573, N'Khánh Vĩnh', 56),
(574, N'Diên Khánh', 56),
(575, N'Khánh Sơn', 56),
(576, N'Trường Sa', 56),
(582, N'Phan Rang-Tháp Chàm', 58),
(584, N'Bác Ái', 58),
(585, N'Ninh Sơn', 58),
(586, N'Ninh Hải', 58),
(587, N'Ninh Phước', 58),
(588, N'Thuận Bắc', 58),
(589, N'Thuận Nam', 58),
(593, N'Phan Thiết', 60),
(594, N'La Gi', 60),
(595, N'Tuy Phong', 60),
(596, N'Bắc Bình', 60),
(597, N'Hàm Thuận Bắc', 60),
(598, N'Hàm Thuận Nam', 60),
(599, N'Tánh Linh', 60),
(600, N'Đức Linh', 60),
(601, N'Hàm Tân', 60),
(602, N'Phú Quí', 60),
(608, N'Kon Tum', 62),
(610, N'Đắk Glei', 62),
(611, N'Ngọc Hồi', 62),
(612, N'Đắk Tô', 62),
(613, N'Kon Plông', 62),
(614, N'Kon Rẫy', 62),
(615, N'Đắk Hà', 62),
(616, N'Sa Thầy', 62),
(617, N'Tu Mơ Rông', 62),
(618, N'Ia H'' Drai', 62),
(622, N'Pleiku', 64),
(623, N'An Khê', 64),
(624, N'Ayun Pa', 64),
(625, N'KBang', 64),
(626, N'Đăk Đoa', 64),
(627, N'Chư Păh', 64),
(628, N'Ia Grai', 64),
(629, N'Mang Yang', 64),
(630, N'Kông Chro', 64),
(631, N'Đức Cơ', 64),
(632, N'Chư Prông', 64),
(633, N'Chư Sê', 64),
(634, N'Đăk Pơ', 64),
(635, N'Ia Pa', 64),
(637, N'Krông Pa', 64),
(638, N'Phú Thiện', 64),
(639, N'Chư Pưh', 64),
(643, N'Buôn Ma Thuột', 66),
(644, N'Buôn Hồ', 66),
(645, N'Ea H''leo', 66),
(646, N'Ea Súp', 66),
(647, N'Buôn Đôn', 66),
(648, N'Cư M''gar', 66),
(649, N'Krông Búk', 66),
(650, N'Krông Năng', 66),
(651, N'Ea Kar', 66),
(652, N'M''Đrắk', 66),
(653, N'Krông Bông', 66),
(654, N'Krông Pắc', 66),
(655, N'Krông A Na', 66),
(656, N'Lắk', 66),
(657, N'Cư Kuin', 66),
(660, N'Gia Nghĩa', 67),
(661, N'Đăk Glong', 67),
(662, N'Cư Jút', 67),
(663, N'Đắk Mil', 67),
(664, N'Krông Nô', 67),
(665, N'Đắk Song', 67),
(666, N'Đắk R''Lấp', 67),
(667, N'Tuy Đức', 67),
(672, N'Đà Lạt', 68),
(673, N'Bảo Lộc', 68),
(674, N'Đam Rông', 68),
(675, N'Lạc Dương', 68),
(676, N'Lâm Hà', 68),
(677, N'Đơn Dương', 68),
(678, N'Đức Trọng', 68),
(679, N'Di Linh', 68),
(680, N'Bảo Lâm', 68),
(682, N'Đạ Tẻh', 68),
(688, N'Phước Long', 70),
(689, N'Đồng Xoài', 70),
(690, N'Bình Long', 70),
(691, N'Bù Gia Mập', 70),
(692, N'Lộc Ninh', 70),
(693, N'Bù Đốp', 70),
(694, N'Hớn Quản', 70),
(695, N'Đồng Phú', 70),
(696, N'Bù Đăng', 70),
(697, N'Chơn Thành', 70),
(698, N'Phú Riềng', 70),
(703, N'Tây Ninh', 72),
(705, N'Tân Biên', 72),
(706, N'Tân Châu', 72),
(707, N'Dương Minh Châu', 72),
(708, N'Châu Thành', 72),
(709, N'Hòa Thành', 72),
(710, N'Gò Dầu', 72),
(711, N'Bến Cầu', 72),
(712, N'Trảng Bàng', 72),
(718, N'Thủ Dầu Một', 74),
(719, N'Bàu Bàng', 74),
(720, N'Dầu Tiếng', 74),
(721, N'Bến Cát', 74),
(722, N'Phú Giáo', 74),
(723, N'Tân Uyên', 74),
(724, N'Dĩ An', 74),
(725, N'Thuận An', 74),
(726, N'Bắc Tân Uyên', 74),
(731, N'Biên Hòa', 75),
(732, N'Long Khánh', 75),
(734, N'Tân Phú', 75),
(735, N'Vĩnh Cửu', 75),
(736, N'Định Quán', 75),
(737, N'Trảng Bom', 75),
(738, N'Thống Nhất', 75),
(739, N'Cẩm Mỹ', 75),
(740, N'Long Thành', 75),
(741, N'Xuân Lộc', 75),
(742, N'Nhơn Trạch', 75),
(747, N'Vũng Tàu', 77),
(748, N'Bà Rịa', 77),
(750, N'Châu Đức', 77),
(751, N'Xuyên Mộc', 77),
(753, N'Long Đất', 77),
(754, N'Phú Mỹ', 77),
(755, N'Côn Đảo', 77),
(760, N'1', 79),
(761, N'12', 79),
(764, N'Gò Vấp', 79),
(765, N'Bình Thạnh', 79),
(766, N'Tân Bình', 79),
(767, N'Tân Phú', 79),
(768, N'Phú Nhuận', 79),
(769, N'Thủ Đức', 79),
(770, N'3', 79),
(771, N'10', 79),
(772, N'11', 79),
(773, N'4', 79),
(774, N'5', 79),
(775, N'6', 79),
(776, N'8', 79),
(777, N'Bình Tân', 79),
(778, N'7', 79),
(783, N'Củ Chi', 79),
(784, N'Hóc Môn', 79),
(785, N'Bình Chánh', 79),
(786, N'Nhà Bè', 79),
(787, N'Cần Giờ', 79),
(794, N'Tân An', 80),
(795, N'Kiến Tường', 80),
(796, N'Tân Hưng', 80),
(797, N'Vĩnh Hưng', 80),
(798, N'Mộc Hóa', 80),
(799, N'Tân Thạnh', 80),
(800, N'Thạnh Hóa', 80),
(801, N'Đức Huệ', 80),
(802, N'Đức Hòa', 80),
(803, N'Bến Lức', 80),
(804, N'Thủ Thừa', 80),
(805, N'Tân Trụ', 80),
(806, N'Cần Đước', 80),
(807, N'Cần Giuộc', 80),
(808, N'Châu Thành', 80),
(815, N'Mỹ Tho', 82),
(816, N'Gò Công', 82),
(817, N'Cai Lậy', 82),
(818, N'Tân Phước', 82),
(819, N'Cái Bè', 82),
(820, N'Cai Lậy', 82),
(821, N'Châu Thành', 82),
(822, N'Chợ Gạo', 82),
(823, N'Gò Công Tây', 82),
(824, N'Gò Công Đông', 82),
(825, N'Tân Phú Đông', 82),
(829, N'Bến Tre', 83),
(831, N'Châu Thành', 83),
(832, N'Chợ Lách', 83),
(833, N'Mỏ Cày Nam', 83),
(834, N'Giồng Trôm', 83),
(835, N'Bình Đại', 83),
(836, N'Ba Tri', 83),
(837, N'Thạnh Phú', 83),
(838, N'Mỏ Cày Bắc', 83),
(842, N'Trà Vinh', 84),
(844, N'Càng Long', 84),
(845, N'Cầu Kè', 84),
(846, N'Tiểu Cần', 84),
(847, N'Châu Thành', 84),
(848, N'Cầu Ngang', 84),
(849, N'Trà Cú', 84),
(850, N'Duyên Hải', 84),
(851, N'Duyên Hải', 84),
(855, N'Vĩnh Long', 86),
(857, N'Long Hồ', 86),
(858, N'Mang Thít', 86),
(859, N'Vũng Liêm', 86),
(860, N'Tam Bình', 86),
(861, N'Bình Minh', 86),
(862, N'Trà Ôn', 86),
(863, N'Bình Tân', 86),
(866, N'Cao Lãnh', 87),
(867, N'Sa Đéc', 87),
(868, N'Hồng Ngự', 87),
(869, N'Tân Hồng', 87),
(870, N'Hồng Ngự', 87),
(871, N'Tam Nông', 87),
(872, N'Tháp Mười', 87),
(873, N'Cao Lãnh', 87),
(874, N'Thanh Bình', 87),
(875, N'Lấp Vò', 87),
(876, N'Lai Vung', 87),
(877, N'Châu Thành', 87),
(883, N'Long Xuyên', 89),
(884, N'Châu Đốc', 89),
(886, N'An Phú', 89),
(887, N'Tân Châu', 89),
(888, N'Phú Tân', 89),
(889, N'Châu Phú', 89),
(890, N'Tịnh Biên', 89),
(891, N'Tri Tôn', 89),
(892, N'Châu Thành', 89),
(893, N'Chợ Mới', 89),
(894, N'Thoại Sơn', 89),
(899, N'Rạch Giá', 91),
(900, N'Hà Tiên', 91),
(902, N'Kiên Lương', 91),
(903, N'Hòn Đất', 91),
(904, N'Tân Hiệp', 91),
(905, N'Châu Thành', 91),
(906, N'Giồng Riềng', 91),
(907, N'Gò Quao', 91),
(908, N'An Biên', 91),
(909, N'An Minh', 91),
(910, N'Vĩnh Thuận', 91),
(911, N'Phú Quốc', 91),
(912, N'Kiên Hải', 91),
(913, N'U Minh Thượng', 91),
(914, N'Giang Thành', 91),
(916, N'Ninh Kiều', 92),
(917, N'Ô Môn', 92),
(918, N'Bình Thuỷ', 92),
(919, N'Cái Răng', 92),
(923, N'Thốt Nốt', 92),
(924, N'Vĩnh Thạnh', 92),
(925, N'Cờ Đỏ', 92),
(926, N'Phong Điền', 92),
(927, N'Thới Lai', 92),
(930, N'Vị Thanh', 93),
(931, N'Ngã Bảy', 93),
(932, N'Châu Thành A', 93),
(933, N'Châu Thành', 93),
(934, N'Phụng Hiệp', 93),
(935, N'Vị Thuỷ', 93),
(936, N'Long Mỹ', 93),
(937, N'Long Mỹ', 93),
(941, N'Sóc Trăng', 94),
(942, N'Châu Thành', 94),
(943, N'Kế Sách', 94),
(944, N'Mỹ Tú', 94),
(945, N'Cù Lao Dung', 94),
(946, N'Long Phú', 94),
(947, N'Mỹ Xuyên', 94),
(948, N'Ngã Năm', 94),
(949, N'Thạnh Trị', 94),
(950, N'Vĩnh Châu', 94),
(951, N'Trần Đề', 94),
(954, N'Bạc Liêu', 95),
(956, N'Hồng Dân', 95),
(957, N'Phước Long', 95),
(958, N'Vĩnh Lợi', 95),
(959, N'Giá Rai', 95),
(960, N'Đông Hải', 95),
(961, N'Hoà Bình', 95),
(964, N'Cà Mau', 96),
(966, N'U Minh', 96),
(967, N'Thới Bình', 96),
(968, N'Trần Văn Thời', 96),
(969, N'Cái Nước', 96),
(970, N'Đầm Dơi', 96),
(971, N'Năm Căn', 96),
(972, N'Phú Tân', 96),
(973, N'Ngọc Hiển', 96);  -- Truncated to avoid SQL size overflow

INSERT INTO Wards(ward_id, ward_name, province_id) VALUES
(00001, N'Phúc Xá', 001),
(00004, N'Trúc Bạch', 001),
(00006, N'Vĩnh Phúc', 001),
(00007, N'Cống Vị', 001),
(00008, N'Liễu Giai', 001),
(00013, N'Quán Thánh', 001),
(00016, N'Ngọc Hà', 001),
(00019, N'Điện Biên', 001),
(00022, N'Đội Cấn', 001),
(00025, N'Ngọc Khánh', 001),
(00028, N'Kim Mã', 001),
(00031, N'Giảng Võ', 001),
(00034, N'Thành Công', 001),
(00037, N'Phúc Tân', 002),
(00040, N'Đồng Xuân', 002),
(00043, N'Hàng Mã', 002),
(00046, N'Hàng Buồm', 002),
(00049, N'Hàng Đào', 002),
(00052, N'Hàng Bồ', 002),
(00055, N'Cửa Đông', 002),
(00058, N'Lý Thái Tổ', 002),
(00061, N'Hàng Bạc', 002),
(00064, N'Hàng Gai', 002),
(00067, N'Chương Dương', 002),
(00070, N'Hàng Trống', 002),
(00073, N'Cửa Nam', 002),
(00076, N'Hàng Bông', 002),
(00079, N'Tràng Tiền', 002),
(00082, N'Trần Hưng Đạo', 002),
(00085, N'Phan Chu Trinh', 002),
(00088, N'Hàng Bài', 002),
(00091, N'Phú Thượng', 003),
(00094, N'Nhật Tân', 003),
(00097, N'Tứ Liên', 003),
(00100, N'Quảng An', 003),
(00103, N'Xuân La', 003),
(00106, N'Yên Phụ', 003),
(00109, N'Bưởi', 003),
(00112, N'Thụy Khuê', 003),
(00115, N'Thượng Thanh', 004),
(00118, N'Ngọc Thụy', 004),
(00121, N'Giang Biên', 004),
(00124, N'Đức Giang', 004),
(00127, N'Việt Hưng', 004),
(00130, N'Gia Thụy', 004),
(00133, N'Ngọc Lâm', 004),
(00136, N'Phúc Lợi', 004),
(00139, N'Bồ Đề', 004),
(00145, N'Long Biên', 004),
(00148, N'Thạch Bàn', 004),
(00151, N'Phúc Đồng', 004),
(00154, N'Cự Khối', 004),
(00157, N'Nghĩa Đô', 005),
(00160, N'Nghĩa Tân', 005),
(00163, N'Mai Dịch', 005),
(00166, N'Dịch Vọng', 005),
(00167, N'Dịch Vọng Hậu', 005),
(00169, N'Quan Hoa', 005),
(00172, N'Yên Hoà', 005),
(00175, N'Trung Hoà', 005),
(00178, N'Cát Linh', 006),
(00181, N'Văn Miếu - Quốc Tử Giám', 006),
(00187, N'Láng Thượng', 006),
(00190, N'Ô Chợ Dừa', 006),
(00193, N'Văn Chương', 006),
(00196, N'Hàng Bột', 006),
(00199, N'Láng Hạ', 006),
(00202, N'Khâm Thiên', 006),
(00205, N'Thổ Quan', 006),
(00208, N'Nam Đồng', 006),
(00883, N'Cán Tỷ', 029),
(00214, N'Quang Trung', 006),
(00217, N'Trung Liệt', 006),
(00226, N'Phương Liên - Trung Tự', 006),
(00229, N'Kim Liên', 006),
(00232, N'Phương Mai', 006),
(00235, N'Thịnh Quang', 006),
(00238, N'Khương Thượng', 006),
(00241, N'Nguyễn Du', 007),
(00244, N'Bạch Đằng', 007),
(00247, N'Phạm Đình Hổ', 007),
(00256, N'Lê Đại Hành', 007),
(00259, N'Đồng Nhân', 007),
(00262, N'Phố Huế', 007),
(00268, N'Thanh Lương', 007),
(00271, N'Thanh Nhàn', 007),
(00277, N'Bách Khoa', 007),
(00280, N'Đồng Tâm', 007),
(00283, N'Vĩnh Tuy', 007),
(00289, N'Quỳnh Mai', 007),
(00292, N'Bạch Mai', 007),
(00295, N'Minh Khai', 007),
(00298, N'Trương Định', 007),
(00301, N'Thanh Trì', 008),
(00304, N'Vĩnh Hưng', 008),
(00307, N'Định Công', 008),
(00310, N'Mai Động', 008),
(00313, N'Tương Mai', 008),
(00316, N'Đại Kim', 008),
(00319, N'Tân Mai', 008),
(00322, N'Hoàng Văn Thụ', 008),
(00325, N'Giáp Bát', 008),
(00328, N'Lĩnh Nam', 008),
(00331, N'Thịnh Liệt', 008),
(00334, N'Trần Phú', 008),
(00337, N'Hoàng Liệt', 008),
(00340, N'Yên Sở', 008),
(00343, N'Nhân Chính', 009),
(00346, N'Thượng Đình', 009),
(00349, N'Khương Trung', 009),
(00352, N'Khương Mai', 009),
(00355, N'Thanh Xuân Trung', 009),
(00358, N'Phương Liệt', 009),
(00364, N'Khương Đình', 009),
(00367, N'Thanh Xuân Bắc', 009),
(00373, N'Hạ Đình', 009),
(00376, N'Sóc Sơn', 016),
(00379, N'Bắc Sơn', 016),
(00382, N'Minh Trí', 016),
(00385, N'Hồng Kỳ', 016),
(00388, N'Nam Sơn', 016),
(00391, N'Trung Giã', 016),
(00394, N'Tân Hưng', 016),
(00397, N'Minh Phú', 016),
(00400, N'Phù Linh', 016),
(00403, N'Bắc Phú', 016),
(00406, N'Tân Minh', 016),
(00409, N'Quang Tiến', 016),
(00412, N'Hiền Ninh', 016),
(00415, N'Tân Dân', 016),
(00418, N'Tiên Dược', 016),
(00421, N'Việt Long', 016),
(00424, N'Xuân Giang', 016),
(00427, N'Mai Đình', 016),
(00430, N'Đức Hoà', 016),
(00433, N'Thanh Xuân', 016),
(00436, N'Đông Xuân', 016),
(00439, N'Kim Lũ', 016),
(00442, N'Phú Cường', 016),
(00445, N'Phú Minh', 016),
(00448, N'Phù Lỗ', 016),
(00451, N'Xuân Thu', 016),
(00454, N'Đông Anh', 017),
(00457, N'Xuân Nộn', 017),
(00460, N'Thuỵ Lâm', 017),
(00463, N'Bắc Hồng', 017),
(00466, N'Nguyên Khê', 017),
(00469, N'Nam Hồng', 017),
(00472, N'Tiên Dương', 017),
(00475, N'Vân Hà', 017),
(00478, N'Uy Nỗ', 017),
(00481, N'Vân Nội', 017),
(00484, N'Liên Hà', 017),
(00487, N'Việt Hùng', 017),
(00490, N'Kim Nỗ', 017),
(00493, N'Kim Chung', 017),
(00496, N'Dục Tú', 017),
(00499, N'Đại Mạch', 017),
(00502, N'Vĩnh Ngọc', 017),
(00505, N'Cổ Loa', 017),
(00508, N'Hải Bối', 017),
(00511, N'Xuân Canh', 017),
(00514, N'Võng La', 017),
(00517, N'Tàm Xá', 017),
(00520, N'Mai Lâm', 017),
(00523, N'Đông Hội', 017),
(00526, N'Yên Viên', 018),
(00529, N'Yên Thường', 018),
(00532, N'Yên Viên', 018),
(00535, N'Ninh Hiệp', 018),
(00541, N'Thiên Đức', 018),
(00544, N'Phù Đổng', 018),
(00550, N'Lệ Chi', 018),
(00553, N'Cổ Bi', 018),
(00556, N'Đặng Xá', 018),
(00562, N'Phú Sơn', 018),
(00565, N'Trâu Quỳ', 018),
(00568, N'Dương Quang', 018),
(00571, N'Dương Xá', 018),
(00577, N'Đa Tốn', 018),
(00580, N'Kiêu Kỵ', 018),
(00583, N'Bát Tràng', 018),
(00589, N'Kim Đức', 018),
(00592, N'Cầu Diễn', 019),
(00622, N'Xuân Phương', 019),
(00623, N'Phương Canh', 019),
(00625, N'Mỹ Đình 1', 019),
(00626, N'Mỹ Đình 2', 019),
(00628, N'Tây Mỗ', 019),
(00631, N'Mễ Trì', 019),
(00632, N'Phú Đô', 019),
(00634, N'Đại Mỗ', 019),
(00637, N'Trung Văn', 019),
(00640, N'Văn Điển', 020),
(00643, N'Tân Triều', 020),
(00646, N'Thanh Liệt', 020),
(00649, N'Tả Thanh Oai', 020),
(00652, N'Hữu Hoà', 020),
(00655, N'Tam Hiệp', 020),
(00658, N'Tứ Hiệp', 020),
(00661, N'Yên Mỹ', 020),
(00664, N'Vĩnh Quỳnh', 020),
(00667, N'Ngũ Hiệp', 020),
(00670, N'Duyên Hà', 020),
(00673, N'Ngọc Hồi', 020),
(00676, N'Vạn Phúc', 020),
(00679, N'Đại áng', 020),
(00682, N'Liên Ninh', 020),
(00685, N'Đông Mỹ', 020),
(00595, N'Thượng Cát', 021),
(00598, N'Liên Mạc', 021),
(00601, N'Đông Ngạc', 021),
(00602, N'Đức Thắng', 021),
(00604, N'Thụy Phương', 021),
(00607, N'Tây Tựu', 021),
(27061, N'2', 768),
(00610, N'Xuân Đỉnh', 021),
(00611, N'Xuân Tảo', 021),
(00613, N'Minh Khai', 021),
(00616, N'Cổ Nhuế 1', 021),
(00617, N'Cổ Nhuế 2', 021),
(00619, N'Phú Diễn', 021),
(00620, N'Phúc Diễn', 021),
(00688, N'Quang Trung', 024),
(00691, N'Trần Phú', 024),
(00692, N'Ngọc Hà', 024),
(00694, N'Nguyễn Trãi', 024),
(00697, N'Minh Khai', 024),
(00700, N'Ngọc Đường', 024),
(00946, N'Phương Độ', 024),
(00949, N'Phương Thiện', 024),
(00712, N'Phó Bảng', 026),
(00715, N'Lũng Cú', 026),
(00718, N'Má Lé', 026),
(00721, N'Đồng Văn', 026),
(00724, N'Lũng Táo', 026),
(00727, N'Phố Là', 026),
(00730, N'Thài Phìn Tủng', 026),
(00733, N'Sủng Là', 026),
(00736, N'Xà Phìn', 026),
(00739, N'Tả Phìn', 026),
(00742, N'Tả Lủng', 026),
(00745, N'Phố Cáo', 026),
(00748, N'Sính Lủng', 026),
(00751, N'Sảng Tủng', 026),
(00754, N'Lũng Thầu', 026),
(00757, N'Hố Quáng Phìn', 026),
(00760, N'Vần Chải', 026),
(00763, N'Lũng Phìn', 026),
(00766, N'Sủng Trái', 026),
(00769, N'Mèo Vạc', 027),
(00772, N'Thượng Phùng', 027),
(00775, N'Pải Lủng', 027),
(00778, N'Xín Cái', 027),
(00781, N'Pả Vi', 027),
(00784, N'Giàng Chu Phìn', 027),
(00787, N'Sủng Trà', 027),
(00790, N'Sủng Máng', 027),
(00793, N'Sơn Vĩ', 027),
(00796, N'Tả Lủng', 027),
(00799, N'Cán Chu Phìn', 027),
(00802, N'Lũng Pù', 027),
(00805, N'Lũng Chinh', 027),
(00808, N'Tát Ngà', 027),
(00811, N'Nậm Ban', 027),
(00814, N'Khâu Vai', 027),
(00815, N'Niêm Tòng', 027),
(00817, N'Niêm Sơn', 027),
(00820, N'Yên Minh', 028),
(00823, N'Thắng Mố', 028),
(00826, N'Phú Lũng', 028),
(00829, N'Sủng Tráng', 028),
(00832, N'Bạch Đích', 028),
(00835, N'Na Khê', 028),
(00838, N'Sủng Thài', 028),
(00841, N'Hữu Vinh', 028),
(00844, N'Lao Và Chải', 028),
(00847, N'Mậu Duệ', 028),
(00850, N'Đông Minh', 028),
(00853, N'Mậu Long', 028),
(00856, N'Ngam La', 028),
(00859, N'Ngọc Long', 028),
(00862, N'Đường Thượng', 028),
(00865, N'Lũng Hồ', 028),
(00868, N'Du Tiến', 028),
(00871, N'Du Già', 028),
(00874, N'Tam Sơn', 029),
(00877, N'Bát Đại Sơn', 029),
(00880, N'Nghĩa Thuận', 029),
(00886, N'Cao Mã Pờ', 029),
(00889, N'Thanh Vân', 029),
(00892, N'Tùng Vài', 029),
(00895, N'Đông Hà', 029),
(00898, N'Quản Bạ', 029),
(00901, N'Lùng Tám', 029),
(00904, N'Quyết Tiến', 029),
(00907, N'Tả Ván', 029),
(00910, N'Thái An', 029),
(00703, N'Kim Thạch', 030),
(00706, N'Phú Linh', 030),
(00709, N'Kim Linh', 030),
(00913, N'Vị Xuyên', 030),
(00916, N'Nông Trường Việt Lâm', 030),
(00919, N'Minh Tân', 030),
(00922, N'Thuận Hoà', 030),
(00925, N'Tùng Bá', 030),
(00928, N'Thanh Thủy', 030),
(00931, N'Thanh Đức', 030),
(00934, N'Phong Quang', 030),
(00937, N'Xín Chải', 030),
(00940, N'Phương Tiến', 030),
(00943, N'Lao Chải', 030),
(00952, N'Cao Bồ', 030),
(00955, N'Đạo Đức', 030),
(00958, N'Thượng Sơn', 030),
(00961, N'Linh Hồ', 030),
(00964, N'Quảng Ngần', 030),
(00967, N'Việt Lâm', 030),
(00970, N'Ngọc Linh', 030),
(00973, N'Ngọc Minh', 030),
(00976, N'Bạch Ngọc', 030),
(00979, N'Trung Thành', 030),
(00982, N'Minh Sơn', 031),
(00985, N'Giáp Trung', 031),
(00988, N'Yên Định', 031),
(00991, N'Yên Phú', 031),
(00994, N'Minh Ngọc', 031),
(00997, N'Yên Phong', 031),
(01000, N'Lạc Nông', 031),
(01003, N'Phú Nam', 031),
(01006, N'Yên Cường', 031),
(01009, N'Thượng Tân', 031),
(01012, N'Đường Âm', 031),
(01015, N'Đường Hồng', 031),
(01018, N'Phiêng Luông', 031),
(01021, N'Vinh Quang', 032),
(01024, N'Bản Máy', 032),
(01027, N'Thàng Tín', 032),
(01030, N'Thèn Chu Phìn', 032),
(01033, N'Pố Lồ', 032),
(01036, N'Bản Phùng', 032),
(01039, N'Túng Sán', 032),
(01042, N'Chiến Phố', 032),
(01045, N'Đản Ván', 032),
(01048, N'Tụ Nhân', 032),
(01051, N'Tân Tiến', 032),
(01054, N'Nàng Đôn', 032),
(01057, N'Pờ Ly Ngài', 032),
(01060, N'Sán Xả Hồ', 032),
(01063, N'Bản Luốc', 032),
(01066, N'Ngàm Đăng Vài', 032),
(01069, N'Bản Nhùng', 032),
(01072, N'Tả Sử Choóng', 032),
(01075, N'Nậm Dịch', 032),
(01081, N'Hồ Thầu', 032),
(01084, N'Nam Sơn', 032),
(01087, N'Nậm Tỵ', 032),
(01090, N'Thông Nguyên', 032),
(01093, N'Nậm Khòa', 032),
(01096, N'Cốc Pài', 033),
(01099, N'Nàn Xỉn', 033),
(27064, N'8', 768),
(01102, N'Bản Díu', 033),
(01105, N'Chí Cà', 033),
(01108, N'Xín Mần', 033),
(01114, N'Thèn Phàng', 033),
(01117, N'Trung Thịnh', 033),
(01120, N'Pà Vầy Sủ', 033),
(01123, N'Cốc Rế', 033),
(01126, N'Thu Tà', 033),
(01129, N'Nàn Ma', 033),
(01132, N'Tả Nhìu', 033),
(01135, N'Bản Ngò', 033),
(01138, N'Chế Là', 033),
(01141, N'Nấm Dẩn', 033),
(01144, N'Quảng Nguyên', 033),
(01147, N'Nà Chì', 033),
(01150, N'Khuôn Lùng', 033),
(01153, N'Việt Quang', 034),
(01156, N'Vĩnh Tuy', 034),
(01159, N'Tân Lập', 034),
(01162, N'Tân Thành', 034),
(01165, N'Đồng Tiến', 034),
(01168, N'Đồng Tâm', 034),
(01171, N'Tân Quang', 034),
(01174, N'Thượng Bình', 034),
(01177, N'Hữu Sản', 034),
(01180, N'Kim Ngọc', 034),
(01183, N'Việt Vinh', 034),
(01186, N'Bằng Hành', 034),
(01189, N'Quang Minh', 034),
(01192, N'Liên Hiệp', 034),
(01195, N'Vô Điếm', 034),
(01198, N'Việt Hồng', 034),
(01201, N'Hùng An', 034),
(01204, N'Đức Xuân', 034),
(01207, N'Tiên Kiều', 034),
(01210, N'Vĩnh Hảo', 034),
(01213, N'Vĩnh Phúc', 034),
(01216, N'Đồng Yên', 034),
(01219, N'Đông Thành', 034),
(01222, N'Xuân Minh', 035),
(01225, N'Tiên Nguyên', 035),
(01228, N'Tân Nam', 035),
(01231, N'Bản Rịa', 035),
(01234, N'Yên Thành', 035),
(01237, N'Yên Bình', 035),
(01240, N'Tân Trịnh', 035),
(01243, N'Tân Bắc', 035),
(01246, N'Bằng Lang', 035),
(01249, N'Yên Hà', 035),
(01252, N'Hương Sơn', 035),
(01255, N'Xuân Giang', 035),
(01258, N'Nà Khương', 035),
(01261, N'Tiên Yên', 035),
(01264, N'Vĩ Thượng', 035),
(01267, N'Sông Hiến', 040),
(01270, N'Sông Bằng', 040),
(01273, N'Hợp Giang', 040),
(01276, N'Tân Giang', 040),
(01279, N'Ngọc Xuân', 040),
(01282, N'Đề Thám', 040),
(01285, N'Hoà Chung', 040),
(01288, N'Duyệt Trung', 040),
(01693, N'Vĩnh Quang', 040),
(01705, N'Hưng Đạo', 040),
(01720, N'Chu Trinh', 040),
(01290, N'Pác Miầu', 042),
(01291, N'Đức Hạnh', 042),
(01294, N'Lý Bôn', 042),
(01296, N'Nam Cao', 042),
(01297, N'Nam Quang', 042),
(01300, N'Vĩnh Quang', 042),
(01303, N'Quảng Lâm', 042),
(01304, N'Thạch Lâm', 042),
(02317, N'Yên Lập', 073),
(01309, N'Vĩnh Phong', 042),
(01312, N'Mông Ân', 042),
(01315, N'Thái Học', 042),
(01316, N'Thái Sơn', 042),
(01318, N'Yên Thổ', 042),
(01321, N'Bảo Lạc', 043),
(01324, N'Cốc Pàng', 043),
(01327, N'Thượng Hà', 043),
(01330, N'Cô Ba', 043),
(01333, N'Bảo Toàn', 043),
(01336, N'Khánh Xuân', 043),
(01339, N'Xuân Trường', 043),
(01342, N'Hồng Trị', 043),
(01343, N'Kim Cúc', 043),
(01345, N'Phan Thanh', 043),
(01348, N'Hồng An', 043),
(01351, N'Hưng Đạo', 043),
(01352, N'Hưng Thịnh', 043),
(01354, N'Huy Giáp', 043),
(01357, N'Đình Phùng', 043),
(01359, N'Sơn Lập', 043),
(01360, N'Sơn Lộ', 043),
(01363, N'Thông Nông', 045),
(01366, N'Cần Yên', 045),
(01367, N'Cần Nông', 045),
(01372, N'Lương Thông', 045),
(01375, N'Đa Thông', 045),
(01378, N'Ngọc Động', 045),
(01381, N'Yên Sơn', 045),
(01384, N'Lương Can', 045),
(01387, N'Thanh Long', 045),
(01392, N'Xuân Hòa', 045),
(01393, N'Lũng Nặm', 045),
(01399, N'Trường Hà', 045),
(01402, N'Cải Viên', 045),
(01411, N'Nội Thôn', 045),
(01414, N'Tổng Cọt', 045),
(01417, N'Sóc Hà', 045),
(01420, N'Thượng Thôn', 045),
(01429, N'Hồng Sỹ', 045),
(01432, N'Quý Quân', 045),
(01435, N'Mã Ba', 045),
(01438, N'Ngọc Đào', 045),
(01447, N'Trà Lĩnh', 047),
(01453, N'Tri Phương', 047),
(01456, N'Quang Hán', 047),
(01462, N'Xuân Nội', 047),
(01465, N'Quang Trung', 047),
(01468, N'Quang Vinh', 047),
(01471, N'Cao Chương', 047),
(01477, N'Trùng Khánh', 047),
(01480, N'Ngọc Khê', 047),
(01481, N'Ngọc Côn', 047),
(01483, N'Phong Nậm', 047),
(01489, N'Đình Phong', 047),
(01495, N'Đàm Thuỷ', 047),
(01498, N'Khâm Thành', 047),
(01501, N'Chí Viễn', 047),
(01504, N'Lăng Hiếu', 047),
(01507, N'Phong Châu', 047),
(01516, N'Trung Phúc', 047),
(01519, N'Cao Thăng', 047),
(01522, N'Đức Hồng', 047),
(01525, N'Đoài Dương', 047),
(01534, N'Minh Long', 048),
(01537, N'Lý Quốc', 048),
(01540, N'Thắng Lợi', 048),
(01543, N'Đồng Loan', 048),
(01546, N'Đức Quang', 048),
(01549, N'Kim Loan', 048),
(01552, N'Quang Long', 048),
(01555, N'An Lạc', 048),
(01558, N'Thanh Nhật', 048),
(01561, N'Vinh Quý', 048),
(01564, N'Thống Nhất', 048),
(01567, N'Cô Ngân', 048),
(01573, N'Thị Hoa', 048),
(01474, N'Quốc Toản', 049),
(01576, N'Quảng Uyên', 049),
(01579, N'Phi Hải', 049),
(01582, N'Quảng Hưng', 049),
(01594, N'Độc Lập', 049),
(01597, N'Cai Bộ', 049),
(01603, N'Phúc Sen', 049),
(01606, N'Chí Thảo', 049),
(01609, N'Tự Do', 049),
(01615, N'Hồng Quang', 049),
(01618, N'Ngọc Động', 049),
(01624, N'Hạnh Phúc', 049),
(01627, N'Tà Lùng', 049),
(01630, N'Bế Văn Đàn', 049),
(01636, N'Cách Linh', 049),
(01639, N'Đại Sơn', 049),
(01645, N'Tiên Thành', 049),
(01648, N'Hoà Thuận', 049),
(01651, N'Mỹ Hưng', 049),
(01654, N'Nước Hai', 051),
(01657, N'Dân Chủ', 051),
(01660, N'Nam Tuấn', 051),
(01666, N'Đại Tiến', 051),
(01669, N'Đức Long', 051),
(01672, N'Ngũ Lão', 051),
(01675, N'Trương Lương', 051),
(01687, N'Hồng Việt', 051),
(01696, N'Hoàng Tung', 051),
(01699, N'Nguyễn Huệ', 051),
(01702, N'Quang Trung', 051),
(01708, N'Bạch Đằng', 051),
(01711, N'Bình Dương', 051),
(01714, N'Lê Chung', 051),
(01723, N'Hồng Nam', 051),
(01726, N'Nguyên Bình', 052),
(01729, N'Tĩnh Túc', 052),
(01732, N'Yên Lạc', 052),
(01735, N'Triệu Nguyên', 052),
(01738, N'Ca Thành', 052),
(01744, N'Vũ Nông', 052),
(01747, N'Minh Tâm', 052),
(01750, N'Thể Dục', 052),
(01756, N'Mai Long', 052),
(01762, N'Vũ Minh', 052),
(01765, N'Hoa Thám', 052),
(01768, N'Phan Thanh', 052),
(01771, N'Quang Thành', 052),
(01774, N'Tam Kim', 052),
(01777, N'Thành Công', 052),
(01780, N'Thịnh Vượng', 052),
(01783, N'Hưng Đạo', 052),
(01786, N'Đông Khê', 053),
(01789, N'Canh Tân', 053),
(01792, N'Kim Đồng', 053),
(01795, N'Minh Khai', 053),
(01801, N'Đức Thông', 053),
(01804, N'Thái Cường', 053),
(01807, N'Vân Trình', 053),
(01810, N'Thụy Hùng', 053),
(01813, N'Quang Trọng', 053),
(01816, N'Trọng Con', 053),
(01819, N'Lê Lai', 053),
(01822, N'Đức Long', 053),
(01828, N'Lê Lợi', 053),
(01831, N'Đức Xuân', 053),
(01834, N'Nguyễn Thị Minh Khai', 058),
(01837, N'Sông Cầu', 058),
(01840, N'Đức Xuân', 058),
(01843, N'Phùng Chí Kiên', 058),
(01846, N'Huyền Tụng', 058),
(01849, N'Dương Quang', 058),
(01852, N'Nông Thượng', 058),
(01855, N'Xuất Hóa', 058),
(01858, N'Bằng Thành', 060),
(01861, N'Nhạn Môn', 060),
(01864, N'Bộc Bố', 060),
(01867, N'Công Bằng', 060),
(01870, N'Giáo Hiệu', 060),
(01873, N'Xuân La', 060),
(01876, N'An Thắng', 060),
(01879, N'Cổ Linh', 060),
(01882, N'Nghiên Loan', 060),
(01885, N'Cao Tân', 060),
(01888, N'Chợ Rã', 061),
(01891, N'Bành Trạch', 061),
(01894, N'Phúc Lộc', 061),
(01897, N'Hà Hiệu', 061),
(01900, N'Cao Thượng', 061),
(01906, N'Khang Ninh', 061),
(01909, N'Nam Mẫu', 061),
(01912, N'Thượng Giáo', 061),
(01915, N'Địa Linh', 061),
(01918, N'Yến Dương', 061),
(01921, N'Chu Hương', 061),
(01924, N'Quảng Khê', 061),
(01927, N'Mỹ Phương', 061),
(01930, N'Hoàng Trĩ', 061),
(01933, N'Đồng Phúc', 061),
(01936, N'Nà Phặc', 062),
(01939, N'Thượng Ân', 062),
(01942, N'Bằng Vân', 062),
(01945, N'Cốc Đán', 062),
(01948, N'Trung Hoà', 062),
(01951, N'Đức Vân', 062),
(01954, N'Vân Tùng', 062),
(01957, N'Thượng Quan', 062),
(01960, N'Hiệp Lực', 062),
(01963, N'Thuần Mang', 062),
(01969, N'Phủ Thông', 063),
(01975, N'Vi Hương', 063),
(01978, N'Sĩ Bình', 063),
(01981, N'Vũ Muộn', 063),
(01984, N'Đôn Phong', 063),
(01990, N'Lục Bình', 063),
(01993, N'Tân Tú', 063),
(01999, N'Nguyên Phúc', 063),
(02002, N'Cao Sơn', 063),
(02005, N'Quân Hà', 063),
(02008, N'Cẩm Giàng', 063),
(02011, N'Mỹ Thanh', 063),
(02014, N'Dương Phong', 063),
(02017, N'Quang Thuận', 063),
(02020, N'Bằng Lũng', 064),
(02023, N'Xuân Lạc', 064),
(02026, N'Nam Cường', 064),
(02029, N'Đồng Lạc', 064),
(02032, N'Tân Lập', 064),
(02035, N'Bản Thi', 064),
(02038, N'Quảng Bạch', 064),
(02041, N'Bằng Phúc', 064),
(02044, N'Yên Thịnh', 064),
(02047, N'Yên Thượng', 064),
(02050, N'Phương Viên', 064),
(02053, N'Ngọc Phái', 064),
(02059, N'Đồng Thắng', 064),
(02062, N'Lương Bằng', 064),
(02065, N'Bằng Lãng', 064),
(02068, N'Đại Sảo', 064),
(02071, N'Nghĩa Tá', 064),
(02077, N'Yên Mỹ', 064),
(02080, N'Bình Trung', 064),
(02083, N'Yên Phong', 064),
(02086, N'Đồng Tâm', 065),
(02089, N'Tân Sơn', 065),
(02092, N'Thanh Vận', 065),
(02095, N'Mai Lạp', 065),
(02098, N'Hoà Mục', 065),
(02101, N'Thanh Mai', 065),
(02104, N'Cao Kỳ', 065),
(02107, N'Nông Hạ', 065),
(02110, N'Yên Cư', 065),
(02113, N'Thanh Thịnh', 065),
(02116, N'Yên Hân', 065),
(02122, N'Như Cố', 065),
(02125, N'Bình Văn', 065),
(02131, N'Quảng Chu', 065),
(02137, N'Văn Vũ', 066),
(02140, N'Văn Lang', 066),
(02143, N'Lương Thượng', 066),
(02146, N'Kim Hỷ', 066),
(02152, N'Cường Lợi', 066),
(02155, N'Yến Lạc', 066),
(02158, N'Kim Lư', 066),
(02161, N'Sơn Thành', 066),
(02170, N'Văn Minh', 066),
(02173, N'Côn Minh', 066),
(02176, N'Cư Lễ', 066),
(02179, N'Trần Phú', 066),
(02185, N'Quang Phong', 066),
(02188, N'Dương Sơn', 066),
(02191, N'Xuân Dương', 066),
(02194, N'Đổng Xá', 066),
(02197, N'Liêm Thuỷ', 066),
(02200, N'Phan Thiết', 070),
(02203, N'Minh Xuân', 070),
(02206, N'Tân Quang', 070),
(02209, N'Tràng Đà', 070),
(02212, N'Nông Tiến', 070),
(02215, N'Ỷ La', 070),
(02216, N'Tân Hà', 070),
(02218, N'Hưng Thành', 070),
(02497, N'Kim Phú', 070),
(02503, N'An Khang', 070),
(02509, N'Mỹ Lâm', 070),
(02512, N'An Tường', 070),
(02515, N'Lưỡng Vượng', 070),
(02521, N'Thái Long', 070),
(02524, N'Đội Cấn', 070),
(02233, N'Phúc Yên', 071),
(02242, N'Xuân Lập', 071),
(02251, N'Khuôn Hà', 071),
(02266, N'Lăng Can', 071),
(02269, N'Thượng Lâm', 071),
(02290, N'Bình An', 071),
(02293, N'Hồng Quang', 071),
(02296, N'Thổ Bình', 071),
(02299, N'Phúc Sơn', 071),
(02302, N'Minh Quang', 071),
(02221, N'Na Hang', 072),
(02227, N'Sinh Long', 072),
(02230, N'Thượng Giáp', 072),
(02239, N'Thượng Nông', 072),
(02245, N'Côn Lôn', 072),
(02248, N'Yên Hoa', 072),
(02254, N'Hồng Thái', 072),
(02260, N'Đà Vị', 072),
(02263, N'Khau Tinh', 072),
(02275, N'Sơn Phú', 072),
(02281, N'Năng Khả', 072),
(02284, N'Thanh Tương', 072),
(02287, N'Vĩnh Lộc', 073),
(02305, N'Trung Hà', 073),
(02308, N'Tân Mỹ', 073),
(02311, N'Hà Lang', 073),
(02314, N'Hùng Mỹ', 073),
(02320, N'Tân An', 073),
(02323, N'Bình Phú', 073),
(02326, N'Xuân Quang', 073),
(02329, N'Ngọc Hội', 073),
(02332, N'Phú Bình', 073),
(02335, N'Hòa Phú', 073),
(02338, N'Phúc Thịnh', 073),
(02341, N'Kiên Đài', 073),
(02344, N'Tân Thịnh', 073),
(02347, N'Trung Hòa', 073),
(02350, N'Kim Bình', 073),
(02353, N'Hòa An', 073),
(02356, N'Vinh Quang', 073),
(02359, N'Tri Phú', 073),
(02362, N'Nhân Lý', 073),
(02365, N'Yên Nguyên', 073),
(02368, N'Linh Phú', 073),
(02371, N'Bình Nhân', 073),
(02374, N'Tân Yên', 074),
(02377, N'Yên Thuận', 074),
(02380, N'Bạch Xa', 074),
(02383, N'Minh Khương', 074),
(02386, N'Yên Lâm', 074),
(02389, N'Minh Dân', 074),
(02392, N'Phù Lưu', 074),
(02395, N'Minh Hương', 074),
(02398, N'Yên Phú', 074),
(02401, N'Tân Thành', 074),
(02404, N'Bình Xa', 074),
(02407, N'Thái Sơn', 074),
(02410, N'Nhân Mục', 074),
(02413, N'Thành Long', 074),
(02416, N'Bằng Cốc', 074),
(02419, N'Thái Hòa', 074),
(02422, N'Đức Ninh', 074),
(02425, N'Hùng Đức', 074),
(02431, N'Quí Quân', 075),
(02434, N'Lực Hành', 075),
(02437, N'Kiến Thiết', 075),
(02440, N'Trung Minh', 075),
(02443, N'Chiêu Yên', 075),
(02446, N'Trung Trực', 075),
(02449, N'Xuân Vân', 075),
(02452, N'Phúc Ninh', 075),
(02455, N'Hùng Lợi', 075),
(02458, N'Trung Sơn', 075),
(02461, N'Tân Tiến', 075),
(02464, N'Tứ Quận', 075),
(02467, N'Đạo Viện', 075),
(02470, N'Tân Long', 075),
(02473, N'Yên Sơn', 075),
(02476, N'Kim Quan', 075),
(02479, N'Lang Quán', 075),
(02482, N'Phú Thịnh', 075),
(02485, N'Công Đa', 075),
(02488, N'Trung Môn', 075),
(02491, N'Chân Sơn', 075),
(02494, N'Thái Bình', 075),
(02500, N'Tiến Bộ', 075),
(02506, N'Mỹ Bằng', 075),
(02518, N'Hoàng Khai', 075),
(02527, N'Nhữ Hán', 075),
(02530, N'Nhữ Khê', 075),
(02533, N'Đội Bình', 075),
(02536, N'Sơn Dương', 076),
(02539, N'Trung Yên', 076),
(02542, N'Minh Thanh', 076),
(02545, N'Tân Trào', 076),
(02548, N'Vĩnh Lợi', 076),
(02551, N'Thượng Ấm', 076),
(02554, N'Bình Yên', 076),
(02557, N'Lương Thiện', 076),
(02560, N'Tú Thịnh', 076),
(02563, N'Cấp Tiến', 076),
(02566, N'Hợp Thành', 076),
(02569, N'Phúc Ứng', 076),
(02572, N'Đông Thọ', 076),
(02575, N'Kháng Nhật', 076),
(02578, N'Hợp Hòa', 076),
(02584, N'Quyết Thắng', 076),
(02587, N'Đồng Quý', 076),
(02590, N'Tân Thanh', 076),
(02596, N'Văn Phú', 076),
(02599, N'Chi Thiết', 076),
(02602, N'Đông Lợi', 076),
(02605, N'Thiện Kế', 076),
(02608, N'Hồng Sơn', 076),
(02611, N'Phú Lương', 076),
(02614, N'Ninh Lai', 076),
(02617, N'Đại Phú', 076),
(02620, N'Sơn Nam', 076),
(02623, N'Hào Phú', 076),
(02626, N'Tam Đa', 076),
(02632, N'Trường Sinh', 076),
(02635, N'Duyên Hải', 080),
(02641, N'Lào Cai', 080),
(02644, N'Cốc Lếu', 080),
(02647, N'Kim Tân', 080),
(02650, N'Bắc Lệnh', 080),
(02653, N'Pom Hán', 080),
(02656, N'Xuân Tăng', 080),
(02658, N'Bình Minh', 080),
(02659, N'Thống Nhất', 080),
(02662, N'Đồng Tuyển', 080),
(02665, N'Vạn Hoà', 080),
(02668, N'Bắc Cường', 080),
(02671, N'Nam Cường', 080),
(02674, N'Cam Đường', 080),
(02677, N'Tả Phời', 080),
(02680, N'Hợp Thành', 080),
(02746, N'Cốc San', 080),
(02683, N'Bát Xát', 082),
(02686, N'A Mú Sung', 082),
(02689, N'Nậm Chạc', 082),
(02692, N'A Lù', 082),
(02695, N'Trịnh Tường', 082),
(02701, N'Y Tý', 082),
(02704, N'Cốc Mỳ', 082),
(02707, N'Dền Sáng', 082),
(02710, N'Bản Vược', 082),
(02713, N'Sàng Ma Sáo', 082),
(02716, N'Bản Qua', 082),
(02719, N'Mường Vi', 082),
(02722, N'Dền Thàng', 082),
(02725, N'Bản Xèo', 082),
(02728, N'Mường Hum', 082),
(02731, N'Trung Lèng Hồ', 082),
(02734, N'Quang Kim', 082),
(02737, N'Pa Cheo', 082),
(02740, N'Nậm Pung', 082),
(02743, N'Phìn Ngan', 082),
(02749, N'Tòng Sành', 082),
(02752, N'Pha Long', 083),
(02755, N'Tả Ngải Chồ', 083),
(02758, N'Tung Chung Phố', 083),
(02761, N'Mường Khương', 083),
(02764, N'Dìn Chin', 083),
(02767, N'Tả Gia Khâu', 083),
(02770, N'Nậm Chảy', 083),
(02773, N'Nấm Lư', 083),
(02776, N'Lùng Khấu Nhin', 083),
(02779, N'Thanh Bình', 083),
(02782, N'Cao Sơn', 083),
(02785, N'Lùng Vai', 083),
(02788, N'Bản Lầu', 083),
(02791, N'La Pan Tẩn', 083),
(02794, N'Tả Thàng', 083),
(02797, N'Bản Sen', 083),
(02800, N'Nàn Sán', 084),
(02803, N'Thào Chư Phìn', 084),
(02806, N'Bản Mế', 084),
(02809, N'Si Ma Cai', 084),
(02812, N'Sán Chải', 084),
(02818, N'Lùng Thẩn', 084),
(02821, N'Cán Cấu', 084),
(02824, N'Sín Chéng', 084),
(02827, N'Quan Hồ Thẩn', 084),
(02836, N'Nàn Xín', 084),
(02839, N'Bắc Hà', 085),
(02842, N'Lùng Cải', 085),
(02848, N'Lùng Phình', 085),
(02851, N'Tả Van Chư', 085),
(02854, N'Tả Củ Tỷ', 085),
(02857, N'Thải Giàng Phố', 085),
(02863, N'Hoàng Thu Phố', 085),
(02866, N'Bản Phố', 085),
(02869, N'Bản Liền', 085),
(02875, N'Na Hối', 085),
(02878, N'Cốc Ly', 085),
(02881, N'Nậm Mòn', 085),
(02884, N'Nậm Đét', 085),
(02887, N'Nậm Khánh', 085),
(02890, N'Bảo Nhai', 085),
(02893, N'Nậm Lúc', 085),
(02896, N'Cốc Lầu', 085),
(02899, N'Bản Cái', 085),
(02902, N'N.T Phong Hải', 086),
(02905, N'Phố Lu', 086),
(02908, N'Tằng Loỏng', 086),
(02911, N'Bản Phiệt', 086),
(02914, N'Bản Cầm', 086),
(02917, N'Thái Niên', 086),
(02920, N'Phong Niên', 086),
(02923, N'Gia Phú', 086),
(02926, N'Xuân Quang', 086),
(02929, N'Sơn Hải', 086),
(02932, N'Xuân Giao', 086),
(02935, N'Trì Quang', 086),
(02938, N'Sơn Hà', 086),
(02944, N'Phú Nhuận', 086),
(02947, N'Phố Ràng', 087),
(02950, N'Tân Tiến', 087),
(02953, N'Nghĩa Đô', 087),
(02956, N'Vĩnh Yên', 087),
(02959, N'Điện Quan', 087),
(02962, N'Xuân Hoà', 087),
(02965, N'Tân Dương', 087),
(02968, N'Thượng Hà', 087),
(02971, N'Kim Sơn', 087),
(02974, N'Cam Cọn', 087),
(02977, N'Minh Tân', 087),
(02980, N'Xuân Thượng', 087),
(02983, N'Việt Tiến', 087),
(02986, N'Yên Sơn', 087),
(02989, N'Bảo Hà', 087),
(02992, N'Lương Sơn', 087),
(02998, N'Phúc Khánh', 087),
(03001, N'Sa Pa', 088),
(03002, N'Sa Pả', 088),
(03003, N'Ô Quý Hồ', 088),
(03004, N'Ngũ Chỉ Sơn', 088),
(03006, N'Phan Si Păng', 088),
(03010, N'Trung Chải', 088),
(03013, N'Tả Phìn', 088),
(03016, N'Hàm Rồng', 088),
(03019, N'Hoàng Liên', 088),
(03022, N'Thanh Bình', 088),
(03028, N'Cầu Mây', 088),
(03037, N'Mường Hoa', 088),
(03040, N'Tả Van', 088),
(03043, N'Mường Bo', 088),
(03046, N'Bản Hồ', 088),
(03052, N'Liên Minh', 088),
(03055, N'Khánh Yên', 089),
(03061, N'Võ Lao', 089),
(03064, N'Sơn Thuỷ', 089),
(03067, N'Nậm Mả', 089),
(03070, N'Tân Thượng', 089),
(03073, N'Nậm Rạng', 089),
(03076, N'Nậm Chầy', 089),
(03079, N'Tân An', 089),
(03082, N'Khánh Yên Thượng', 089),
(03085, N'Nậm Xé', 089),
(03088, N'Dần Thàng', 089),
(03091, N'Chiềng Ken', 089),
(03094, N'Làng Giàng', 089),
(03097, N'Hoà Mạc', 089),
(03100, N'Khánh Yên Trung', 089),
(03103, N'Khánh Yên Hạ', 089),
(03106, N'Dương Quỳ', 089),
(03109, N'Nậm Tha', 089),
(03112, N'Minh Lương', 089),
(03115, N'Thẩm Dương', 089),
(03118, N'Liêm Phú', 089),
(03121, N'Nậm Xây', 089),
(03124, N'Noong Bua', 094),
(03127, N'Him Lam', 094),
(03130, N'Thanh Bình', 094),
(03133, N'Tân Thanh', 094),
(03136, N'Mường Thanh', 094),
(03139, N'Nam Thanh', 094),
(03142, N'Thanh Trường', 094),
(03145, N'Thanh Minh', 094),
(03316, N'Nà Tấu', 094),
(03317, N'Nà Nhạn', 094),
(03325, N'Mường Phăng', 094),
(03326, N'Pá Khoang', 094),
(03148, N'Sông Đà', 095),
(03151, N'Na Lay', 095),
(03184, N'Lay Nưa', 095),
(03154, N'Sín Thầu', 096),
(03155, N'Sen Thượng', 096),
(03157, N'Chung Chải', 096),
(03158, N'Leng Su Sìn', 096),
(03159, N'Pá Mỳ', 096),
(03160, N'Mường Nhé', 096),
(03161, N'Nậm Vì', 096),
(03162, N'Nậm Kè', 096),
(03163, N'Mường Toong', 096),
(03164, N'Quảng Lâm', 096),
(03177, N'Huổi Lếnh', 096),
(03172, N'Mường Chà', 097);  -- Truncated to avoid SQL size overflow

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

SET IDENTITY_INSERT [dbo].[News] ON 

INSERT [dbo].[News] ([news_id], [title], [news_content], [author_id], [post_time], [image]) VALUES (1, N'<p>Học l&aacute;i xe &ocirc; t&ocirc; hạng B1&nbsp;</p>
', N'<p>1. Điều kiện với người học l&aacute;i xe &ocirc; t&ocirc; hạng B1.&nbsp;</p>

<ul>
	<li>
	<p>L&agrave; c&ocirc;ng d&acirc;n Việt Nam, người nước ngo&agrave;i được ph&eacute;p cư tr&uacute; hoặc đang l&agrave;m việc, học tập tại Việt Nam.&nbsp;</p>
	</li>
	<li>
	<p>Đủ 18 tuổi (t&iacute;nh đến ng&agrave;y dự thi s&aacute;t hạch l&aacute;i xe), đủ điều kiện sức khoẻ theo quy định.</p>
	</li>
</ul>

<p>&nbsp;2. Hồ sơ của người học l&aacute;i xe &ocirc; t&ocirc; B1.&nbsp;</p>

<ul>
	<li>
	<p>Bản sao giấy chứng minh hoặc căn cước, trường hợp l&agrave;m mất giấy tờ c&oacute; thể thay thế bằng bản sao hộ chiếu c&ograve;n hạn sử dụng;&nbsp;</p>
	</li>
	<li>
	<p>Giấy kiểm tra sức khỏe từ c&aacute;c cơ sở y tế c&oacute; thẩm quyền cấp;</p>
	</li>
	<li>
	<p>Tờ đơn đề nghị học, thi s&aacute;t hạch v&agrave; cấp giấy ph&eacute;p l&aacute;i xe&nbsp;B1;</p>
	</li>
	<li>
	<p>Ảnh thẻ 3x4 (nền ảnh m&agrave;u xanh, người kh&ocirc;ng đeo k&iacute;nh, mặc &aacute;o c&oacute; cổ).&nbsp;</p>
	</li>
</ul>

<p>3. Thời gian v&agrave; học ph&iacute; học l&aacute;i xe B1.&nbsp;</p>

<ul>
	<li>
	<p>Thời gian kh&oacute;a học:&nbsp;3&nbsp;th&aacute;ng.&nbsp;</p>
	</li>
	<li>
	<p>Học ph&iacute; đ&agrave;o tạo: 5.000.000 VNĐ</p>
	</li>
</ul>

<p>4. Lệ ph&iacute; thi v&agrave; lệ ph&iacute; cấp GPLX.&nbsp;&nbsp;</p>

<ul>
	<li>
	<p>Phần thi l&yacute; thuyết:&nbsp;100.000 VNĐ&nbsp;&nbsp;</p>
	</li>
	<li>
	<p>Phần thi m&ocirc; phỏng:&nbsp;100.000 VNĐ</p>
	</li>
	<li>
	<p>Phần thi thực h&agrave;nh:&nbsp;350.000 VNĐ&nbsp;&nbsp;</p>
	</li>
	<li>
	<p>Phần thi đường trường:&nbsp;80.000 VNĐ&nbsp;</p>
	</li>
	<li>
	<p>Lệ ph&iacute; cấp GPLX:&nbsp;135.000 VNĐ&nbsp;&nbsp;</p>
	</li>
</ul>

<p>&nbsp;&nbsp;Tổng lệ ph&iacute; thi:&nbsp;765.000 VNĐ&nbsp;</p>

<p>5. Lệ ph&iacute; thu&ecirc; xe s&aacute;t hạch trong ng&agrave;y tổng &ocirc;n.&nbsp;</p>

<p>Trước ng&agrave;y thi s&aacute;t hạch l&aacute;i xe Trung t&acirc;m sẽ tổ chức một số ng&agrave;y tổng &ocirc;n luyện gi&uacute;p học vi&ecirc;n &ocirc;n luyện lại kỹ năng l&aacute;i xe tr&ecirc;n xe s&aacute;t hạch c&oacute; thiết bị chấm điểm tự động.</p>

<p>Chi ph&iacute; thu&ecirc; xe để &ocirc;n luyện :</p>

<ul>
	<li>
	<p>Hạng B2 :&nbsp;350.000 VNĐ/tiếng</p>
	</li>
</ul>

<p>Ch&uacute; &yacute;:&nbsp;Hồ sơ nộp v&agrave;o Trung t&acirc;m qua địa email:&nbsp;<a href="mailto:info@laixebacha.com">laixebacha@gmail.com</a>&nbsp;gồm (04 ảnh 3 x 4 v&agrave; chứng minh thư nh&acirc;n d&acirc;n scan) sau khi nộp hồ sơ bạn sẽ được th&ocirc;ng b&aacute;o chi tiết về lịch học, lịch thi.</p>

<p>&nbsp;</p>

<p>Để được tư vấn trực tiếp vui l&ograve;ng li&ecirc;n hệ:</p>

<p>TRUNG T&Acirc;M Đ&Agrave;O TẠO &amp; S&Aacute;T HẠCH L&Aacute;I XE BẮC H&Agrave;</p>

<p>Hotline:&nbsp;&nbsp;0934.333.000</p>

<p>Email:&nbsp;<a href="mailto:info@laixebacha.com">laixebacha@gmail.com</a></p>

<p>Website:&nbsp;http://laixebacha.com</p>

<p>Địa chỉ:&nbsp;89, Ng&ocirc; Gia Tự, Phường Tam Sơn, Th&agrave;nh phố Từ Sơn, Tỉnh Bắc Ninh</p>
', 4, CAST(N'2025-07-19T10:11:56.483' AS DateTime), N'images/news1.jpg')
INSERT [dbo].[News] ([news_id], [title], [news_content], [author_id], [post_time], [image]) VALUES (2, N'<p>Học l&aacute;i xe &ocirc; t&ocirc; hạng C</p>
', N'<p>1. Điều kiện với người học l&aacute;i xe &ocirc; t&ocirc; hạng C.&nbsp;</p>

<ul>
	<li>
	<p>L&agrave; c&ocirc;ng d&acirc;n Việt Nam, người nước ngo&agrave;i được ph&eacute;p cư tr&uacute; hoặc đang l&agrave;m việc, học tập tại Việt Nam.&nbsp;</p>
	</li>
	<li>
	<p>Đủ 21 tuổi (t&iacute;nh đến ng&agrave;y dự thi s&aacute;t hạch l&aacute;i xe), đủ điều kiện sức khoẻ theo quy định.&nbsp;</p>
	</li>
</ul>

<p><img alt="" src="http://laixebacha.com/uploads/news/c22.PNG" /></p>

<p>2. Hồ sơ của người học l&aacute;i xe &ocirc; t&ocirc; C.&nbsp;</p>

<ul>
	<li>
	<p>Bản sao giấy chứng minh hoặc căn cước, trường hợp l&agrave;m mất giấy tờ c&oacute; thể thay thế bằng bản sao hộ chiếu c&ograve;n hạn sử dụng;</p>
	</li>
	<li>
	<p>Bản sao hộ chiếu c&ograve;n hạn tr&ecirc;n 06 th&aacute;ng v&agrave; thẻ tạm tr&uacute;/thường tr&uacute; hoặc chứng minh thư ngoại giao/c&ocirc;ng vụ đối với người nước ngo&agrave;i cư tr&uacute;, l&agrave;m việc tại Việt Nam;</p>
	</li>
	<li>
	<p>Giấy kiểm tra sức khỏe từ c&aacute;c cơ sở y tế c&oacute; thẩm quyền cấp;</p>
	</li>
	<li>
	<p>Tờ đơn đề nghị học, thi s&aacute;t hạch v&agrave; cấp giấy ph&eacute;p l&aacute;i xe&nbsp;C;</p>
	</li>
	<li>
	<p>Ảnh thẻ 3x4 (nền ảnh m&agrave;u xanh, người kh&ocirc;ng đeo k&iacute;nh, mặc &aacute;o c&oacute; cổ).&nbsp;</p>
	</li>
</ul>

<p>3. Thời gian v&agrave; học ph&iacute; học l&aacute;i xe C.&nbsp;</p>

<ul>
	<li>
	<p>Thời gian kh&oacute;a học:&nbsp;6 th&aacute;ng.&nbsp;</p>
	</li>
	<li>
	<p>Học ph&iacute; đ&agrave;o tạo:&nbsp;9.000.000 VNĐ&nbsp;</p>
	</li>
</ul>

<p>4. Lệ ph&iacute; thi v&agrave; lệ ph&iacute; cấp GPLX.&nbsp;&nbsp;</p>

<ul>
	<li>
	<p>Phần thi l&yacute; thuyết:&nbsp;100.000 VNĐ&nbsp;&nbsp;</p>
	</li>
	<li>
	<p>Phần thi m&ocirc; phỏng:&nbsp;100.000 VNĐ</p>
	</li>
	<li>
	<p>Phần thi thực h&agrave;nh:&nbsp;350.000 VNĐ&nbsp;&nbsp;</p>
	</li>
	<li>
	<p>Phần thi đường trường:&nbsp;80.000 VNĐ&nbsp;</p>
	</li>
	<li>
	<p>Lệ ph&iacute; cấp GPLX:&nbsp;135.000 VNĐ&nbsp;&nbsp;</p>
	</li>
</ul>

<p>&nbsp;&nbsp;Tổng lệ ph&iacute; thi:&nbsp;765.000 VNĐ&nbsp;</p>

<p>5. Lệ ph&iacute; thu&ecirc; xe s&aacute;t hạch trong ng&agrave;y tổng &ocirc;n.&nbsp;</p>

<p>&nbsp;Trước ng&agrave;y thi s&aacute;t hạch l&aacute;i xe Trung t&acirc;m sẽ tổ chức một số ng&agrave;y tổng &ocirc;n luyện gi&uacute;p học vi&ecirc;n &ocirc;n luyện lại kỹ năng l&aacute;i xe tr&ecirc;n xe s&aacute;t hạch c&oacute; thiết bị chấm điểm tự động.</p>

<p>&nbsp;Chi ph&iacute; thu&ecirc; xe để &ocirc;n luyện:</p>

<p>Hạng C :&nbsp;350.000 VNĐ/tiếng</p>

<p>Ch&uacute; &yacute;:&nbsp;Hồ sơ nộp v&agrave;o Trung t&acirc;m qua địa email:&nbsp;<a href="mailto:info@laixebacha.com">laixebacha@gmail.com</a>&nbsp;gồm (04 ảnh 3 x 4 v&agrave; chứng minh thư nh&acirc;n d&acirc;n scan) sau khi nộp hồ sơ bạn sẽ được th&ocirc;ng b&aacute;o chi tiết về lịch học, lịch thi.</p>

<p>&nbsp;</p>

<p>Để được tư vấn trực tiếp vui l&ograve;ng li&ecirc;n hệ:</p>

<p>TRUNG T&Acirc;M Đ&Agrave;O TẠO &amp; S&Aacute;T HẠCH L&Aacute;I XE BẮC H&Agrave;</p>

<p>Hotline:&nbsp;0934.333.000</p>

<p>Email:&nbsp;<a href="mailto:info@laixebacha.com">laixebacha@gmail.com</a></p>

<p>Website:&nbsp;http://laixebacha.com</p>

<p>Địa chỉ:&nbsp;89, Ng&ocirc; Gia Tự, Phường Tam Sơn, Th&agrave;nh phố Từ Sơn, Tỉnh Bắc Ninh</p>
', 4, CAST(N'2025-07-19T10:10:17.230' AS DateTime), N'images/news2.jpg')
INSERT [dbo].[News] ([news_id], [title], [news_content], [author_id], [post_time], [image]) VALUES (3, N'<p>Học l&aacute;i xe &ocirc; t&ocirc; hạng B2</p>
', N'<p>1. Điều kiện với người học l&aacute;i xe &ocirc; t&ocirc; hạng B2.&nbsp;</p>

<ul>
	<li>
	<p>L&agrave; c&ocirc;ng d&acirc;n Việt Nam, người nước ngo&agrave;i được ph&eacute;p cư tr&uacute; hoặc đang l&agrave;m việc, học tập tại Việt Nam.&nbsp;</p>
	</li>
	<li>
	<p>Đủ 18 tuổi (t&iacute;nh đến ng&agrave;y dự thi s&aacute;t hạch l&aacute;i xe), đủ điều kiện sức khoẻ theo quy định.</p>
	</li>
</ul>

<p>&nbsp;2. Hồ sơ của người học l&aacute;i xe &ocirc; t&ocirc; B2.&nbsp;</p>

<ul>
	<li>
	<p>Bản sao giấy chứng minh hoặc căn cước, trường hợp l&agrave;m mất giấy tờ c&oacute; thể thay thế bằng bản sao hộ chiếu c&ograve;n hạn sử dụng;&nbsp;</p>
	</li>
	<li>
	<p>Giấy kiểm tra sức khỏe từ c&aacute;c cơ sở y tế c&oacute; thẩm quyền cấp;</p>
	</li>
	<li>
	<p>Tờ đơn đề nghị học, thi s&aacute;t hạch v&agrave; cấp giấy ph&eacute;p l&aacute;i xe&nbsp;B2;</p>
	</li>
	<li>
	<p>Ảnh thẻ 3x4 (nền ảnh m&agrave;u xanh, người kh&ocirc;ng đeo k&iacute;nh, mặc &aacute;o c&oacute; cổ).&nbsp;</p>
	</li>
</ul>

<p>3. Thời gian v&agrave; học ph&iacute; học l&aacute;i xe B2.&nbsp;</p>

<ul>
	<li>
	<p>Thời gian kh&oacute;a học:&nbsp;3&nbsp;th&aacute;ng.&nbsp;</p>
	</li>
	<li>
	<p>Học ph&iacute; đ&agrave;o tạo:&nbsp;7.000.000 VNĐ</p>
	</li>
</ul>

<p>4. Lệ ph&iacute; thi v&agrave; lệ ph&iacute; cấp GPLX.&nbsp;&nbsp;</p>

<ul>
	<li>
	<p>Phần thi l&yacute; thuyết:&nbsp;100.000 VNĐ&nbsp;&nbsp;</p>
	</li>
	<li>
	<p>Phần thi m&ocirc; phỏng:&nbsp;100.000 VNĐ</p>
	</li>
	<li>
	<p>Phần thi thực h&agrave;nh:&nbsp;350.000 VNĐ&nbsp;&nbsp;</p>
	</li>
	<li>
	<p>Phần thi đường trường:&nbsp;80.000 VNĐ&nbsp;</p>
	</li>
	<li>
	<p>Lệ ph&iacute; cấp GPLX:&nbsp;135.000 VNĐ&nbsp;&nbsp;</p>
	</li>
</ul>

<p>&nbsp;&nbsp;Tổng lệ ph&iacute; thi:&nbsp;765.000 VNĐ&nbsp;</p>

<p>5. Lệ ph&iacute; thu&ecirc; xe s&aacute;t hạch trong ng&agrave;y tổng &ocirc;n.&nbsp;</p>

<p>Trước ng&agrave;y thi s&aacute;t hạch l&aacute;i xe Trung t&acirc;m sẽ tổ chức một số ng&agrave;y tổng &ocirc;n luyện gi&uacute;p học vi&ecirc;n &ocirc;n luyện lại kỹ năng l&aacute;i xe tr&ecirc;n xe s&aacute;t hạch c&oacute; thiết bị chấm điểm tự động.</p>

<p>Chi ph&iacute; thu&ecirc; xe để &ocirc;n luyện :</p>

<ul>
	<li>
	<p>Hạng B2 :&nbsp;350.000 VNĐ/tiếng</p>
	</li>
</ul>

<p>Ch&uacute; &yacute;:&nbsp;Hồ sơ nộp v&agrave;o Trung t&acirc;m qua địa email:&nbsp;<a href="mailto:info@laixebacha.com">laixebacha@gmail.com</a>&nbsp;gồm (04 ảnh 3 x 4 v&agrave; chứng minh thư nh&acirc;n d&acirc;n scan) sau khi nộp hồ sơ bạn sẽ được th&ocirc;ng b&aacute;o chi tiết về lịch học, lịch thi.</p>

<p>&nbsp;</p>

<p>Để được tư vấn trực tiếp vui l&ograve;ng li&ecirc;n hệ:</p>

<p>TRUNG T&Acirc;M Đ&Agrave;O TẠO &amp; S&Aacute;T HẠCH L&Aacute;I XE BẮC H&Agrave;</p>

<p>Hotline:&nbsp;&nbsp;0934.333.000</p>

<p>Email:&nbsp;<a href="mailto:info@laixebacha.com">laixebacha@gmail.com</a></p>

<p>Website:&nbsp;http://laixebacha.com</p>

<p>Địa chỉ:&nbsp;89, Ng&ocirc; Gia Tự, Phường Tam Sơn, Th&agrave;nh phố Từ Sơn, Tỉnh Bắc Ninh</p>
', 5, CAST(N'2025-07-19T10:10:08.910' AS DateTime), N'images/news3.jpg')
INSERT [dbo].[News] ([news_id], [title], [news_content], [author_id], [post_time], [image]) VALUES (4, N'<p>Học bằng l&aacute;i xe hạng D</p>
', N'<p>1. Điều kiện với người học l&aacute;i xe &ocirc; t&ocirc; hạng D.&nbsp;</p>

<ul>
	<li>
	<p>Độ tuổi:&nbsp;Người học phải đủ 24 tuổi trở l&ecirc;n.&nbsp;</p>
	</li>
	<li>
	<p>Bằng l&aacute;i xe:&nbsp;Cần c&oacute; bằng l&aacute;i xe hạng B2 hoặc C.&nbsp;</p>
	</li>
	<li>
	<p>Kinh nghiệm l&aacute;i xe:&nbsp;Cần c&oacute; kinh nghiệm l&aacute;i xe &iacute;t nhất 3 năm v&agrave; số km l&aacute;i xe an to&agrave;n theo quy định.&nbsp;</p>
	</li>
</ul>

<p>&nbsp;2. Nội dung đ&agrave;o tạo:</p>

<ul>
	<li>
	<p>Học l&yacute; thuyết về luật giao th&ocirc;ng đường bộ, cấu tạo v&agrave; sửa chữa th&ocirc;ng thường xe &ocirc; t&ocirc;, nghiệp vụ vận tải, kỹ thuật l&aacute;i xe, v&agrave; c&aacute;c t&igrave;nh huống giao th&ocirc;ng thường gặp.</p>
	</li>
	<li>
	<p>Học thực h&agrave;nh tr&ecirc;n s&acirc;n tập v&agrave; tr&ecirc;n đường giao th&ocirc;ng, tập trung v&agrave;o c&aacute;c kỹ năng l&aacute;i xe an to&agrave;n, xử l&yacute; c&aacute;c t&igrave;nh huống khẩn cấp v&agrave; điều khiển xe kh&aacute;ch.&nbsp;</p>
	</li>
</ul>

<p>3. Thời gian đ&agrave;o tạo:</p>

<ul>
	<li>
	<p>Thời gian học thực h&agrave;nh v&agrave; l&yacute; thuyết khoảng 336 giờ.</p>
	</li>
	<li>
	<p>Sau khi ho&agrave;n th&agrave;nh kh&oacute;a học, học vi&ecirc;n sẽ tham gia kỳ thi s&aacute;t hạch do Sở GTVT tổ chức.&nbsp;</p>
	</li>
</ul>

<p>4. Chi ph&iacute; đ&agrave;o tạo:</p>

<ul>
	<li>
	<p>Chi ph&iacute; đ&agrave;o tạo n&acirc;ng hạng từ B2 l&ecirc;n D khoảng hơn 5.500.000 đồng/bộ hồ sơ.</p>
	</li>
	<li>
	<p>Bao gồm học ph&iacute; v&agrave; lệ ph&iacute; thi s&aacute;t hạch.&nbsp;</p>
	</li>
</ul>

<p>Lưu &yacute;:</p>

<ul>
	<li>
	<p>Khi c&oacute; bằng l&aacute;i xe hạng D, bạn c&oacute; thể l&aacute;i c&aacute;c loại xe kh&aacute;ch từ 10 đến 30 chỗ ngồi, bao gồm cả xe kh&aacute;ch giường nằm.&nbsp;</p>
	</li>
	<li>
	<p>Bạn cũng được ph&eacute;p l&aacute;i c&aacute;c loại xe tương đương với hạng B2 v&agrave; C.&nbsp;</p>
	</li>
	<li>
	<p>Bạn c&oacute; thể tham khảo c&aacute;c s&acirc;n tập l&aacute;i xe v&agrave; trung t&acirc;m đ&agrave;o tạo l&aacute;i xe uy t&iacute;n tại H&agrave; Nội để đăng k&yacute; kh&oacute;a học.&nbsp;</p>
	</li>
</ul>
', 5, CAST(N'2025-07-19T10:09:57.503' AS DateTime), N'images/news5.jpg')
INSERT [dbo].[News] ([news_id], [title], [news_content], [author_id], [post_time], [image]) VALUES (5, N'<p>Kh&oacute;a đ&agrave;o tạo l&aacute;i xe hạng E</p>
', N'<p>Kh&oacute;a đ&agrave;o tạo l&aacute;i xe hạng E&nbsp;l&agrave; kh&oacute;a học n&acirc;ng hạng bằng l&aacute;i xe, cho ph&eacute;p người l&aacute;i điều khiển c&aacute;c loại xe &ocirc; t&ocirc; chở người tr&ecirc;n 30 chỗ (kh&ocirc;ng kể chỗ người l&aacute;i) v&agrave; c&aacute;c loại xe quy định cho c&aacute;c hạng B1, B2, C, D. Để tham gia kh&oacute;a học n&agrave;y, người học cần đ&aacute;p ứng c&aacute;c điều kiện nhất định về độ tuổi v&agrave; kinh nghiệm l&aacute;i xe.&nbsp;</p>

<p>Điều kiện tham gia kh&oacute;a đ&agrave;o tạo l&aacute;i xe hạng E:</p>

<ul>
	<li>
	<p><strong>Độ tuổi:</strong>&nbsp;Người học phải đủ 27 tuổi trở l&ecirc;n t&iacute;nh đến ng&agrave;y dự s&aacute;t hạch.&nbsp;</p>
	</li>
	<li>
	<p><strong>Kinh nghiệm l&aacute;i xe:</strong></p>

	<ul>
		<li>
		<p>Từ hạng C l&ecirc;n E: C&oacute; 05 năm kinh nghiệm l&aacute;i xe &ocirc; t&ocirc; v&agrave; 100.000 km l&aacute;i xe an to&agrave;n.&nbsp;</p>
		</li>
		<li>
		<p>Từ hạng D l&ecirc;n E: C&oacute; 03 năm kinh nghiệm l&aacute;i xe &ocirc; t&ocirc; v&agrave; 50.000 km l&aacute;i xe an to&agrave;n.&nbsp;</p>
		</li>
	</ul>
	</li>
	<li>
	<p><strong>Sức khỏe:</strong>&nbsp;Đảm bảo đủ điều kiện sức khỏe theo quy định.&nbsp;</p>
	</li>
</ul>

<p>Nội dung kh&oacute;a học:</p>

<ul>
	<li>
	<p><strong>Thời gian đ&agrave;o tạo:</strong></p>

	<p>Tổng thời gian học l&agrave; 336 giờ, bao gồm 56 giờ học l&yacute; thuyết v&agrave; 280 giờ học thực h&agrave;nh.</p>
	</li>
	<li>
	<p><strong>Nội dung học:</strong></p>

	<ul>
		<li>
		<p>Học l&yacute; thuyết về luật giao th&ocirc;ng đường bộ, kỹ thuật l&aacute;i xe, cấu tạo v&agrave; sửa chữa xe, v&agrave; c&aacute;c kiến thức li&ecirc;n quan kh&aacute;c.</p>
		</li>
		<li>
		<p>Thực h&agrave;nh l&aacute;i xe tr&ecirc;n c&aacute;c loại xe hạng E, bao gồm cả l&aacute;i xe trong h&igrave;nh v&agrave; l&aacute;i xe tr&ecirc;n đường trường.&nbsp;</p>
		</li>
	</ul>
	</li>
</ul>

<p>Lưu &yacute;:</p>

<ul>
	<li>
	<p>Người học cần chuẩn bị đầy đủ hồ sơ theo quy định của cơ sở đ&agrave;o tạo l&aacute;i xe.&nbsp;</p>
	</li>
	<li>
	<p>Chi ph&iacute; học bằng l&aacute;i xe hạng E c&oacute; thể dao động t&ugrave;y thuộc v&agrave;o cơ sở đ&agrave;o tạo v&agrave; c&aacute;c dịch vụ đi k&egrave;m.&nbsp;</p>
	</li>
	<li>
	<p>Sau khi ho&agrave;n th&agrave;nh kh&oacute;a học v&agrave; đạt kết quả s&aacute;t hạch, người học sẽ được cấp bằng l&aacute;i xe hạng E.&nbsp;</p>
	</li>
</ul>
', 6, CAST(N'2025-07-19T10:09:34.133' AS DateTime), N'images/news7.jpg')
SET IDENTITY_INSERT [dbo].[News] OFF
