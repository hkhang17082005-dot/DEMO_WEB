INSERT INTO Locations (LocationName) VALUES
(N'Hà Nội'),
(N'Hồ Chí Minh'),
(N'Đà Nẵng'),
(N'Hải Phòng'),
(N'Cần Thơ'),
(N'An Giang'),
(N'Bà Rịa - Vũng Tàu'),
(N'Bắc Giang'),
(N'Bắc Kạn'),
(N'Bạc Liêu'),
(N'Bắc Ninh'),
(N'Bến Tre'),
(N'Bình Định'),
(N'Bình Dương'),
(N'Bình Phước'),
(N'Bình Thuận'),
(N'Cà Mau'),
(N'Cao Bằng'),
(N'Đắk Lắk'),
(N'Đắk Nông'),
(N'Điện Biên'),
(N'Đồng Nai'),
(N'Đồng Tháp'),
(N'Gia Lai'),
(N'Hà Giang'),
(N'Hà Nam'),
(N'Hà Tĩnh'),
(N'Hải Dương'),
(N'Hậu Giang'),
(N'Hòa Bình'),
(N'Hưng Yên'),
(N'Khánh Hòa'),
(N'Kiên Giang'),
(N'Kon Tum'),
(N'Lai Châu'),
(N'Lâm Đồng'),
(N'Lạng Sơn'),
(N'Lào Cai'),
(N'Long An'),
(N'Nam Định'),
(N'Nghệ An'),
(N'Ninh Bình'),
(N'Ninh Thuận'),
(N'Phú Thọ'),
(N'Phú Yên'),
(N'Quảng Bình'),
(N'Quảng Nam'),
(N'Quảng Ngãi'),
(N'Quảng Ninh'),
(N'Quảng Trị'),
(N'Sóc Trăng'),
(N'Sơn La'),
(N'Tây Ninh'),
(N'Thái Bình'),
(N'Thái Nguyên'),
(N'Thanh Hóa'),
(N'Thừa Thiên Huế'),
(N'Tiền Giang'),
(N'Trà Vinh'),
(N'Tuyên Quang'),
(N'Vĩnh Long'),
(N'Vĩnh Phúc'),
(N'Yên Bái');


INSERT INTO Jobs (Title, CompanyName, Salary, JobType, LocationID, LogoUrl)
VALUES
(N'Backend Developer .NET', N'Shoppe', N'15 - 25 triệu', N'Full-time', 1, N'/uploads/logos/shopee.png'),
(N'Frontend Developer React', N'Công ty Tiki', N'12 - 20 triệu', N'Full-time', 2, N'/uploads/logos/tiki.png'),
(N'Designer UI/UX', N'Công ty VNG', N'10 - 18 triệu', N'Part-time', 3, N'/uploads/logos/vng.png'),
(N'Kế toán tổng hợp', N'Công ty Tài Chính A', N'8 - 15 triệu', N'Full-time', 1, N'/uploads/logos/taichinh.png'),
(N'Chuyên viên Marketing', N'Công ty Marketing B', N'10 - 20 triệu', N'Full-time', 2, N'/uploads/logos/maketing.png');

INSERT INTO JobDetails 
(JobID, Description, Requirements, Benefits, WorkAddress, WorkTime, Quantity, Experience, Gender, Deadline)
VALUES

-- JOB 1
(1,
N'Phát triển hệ thống backend bằng .NET Core, xây dựng RESTful API và tối ưu hiệu năng hệ thống.

- Thiết kế database và xử lý logic nghiệp vụ
- Tích hợp API với frontend
- Bảo trì và nâng cấp hệ thống',

N'- Có kinh nghiệm C#, .NET Core
- Hiểu biết về SQL Server
- Tư duy logic tốt',

N'- Lương tháng 13 + thưởng KPI
- Du lịch 2 lần/năm
- Bảo hiểm sức khỏe cao cấp
- Hỗ trợ học tập nâng cao',

N'123 Nguyễn Trãi, Thanh Xuân, Hà Nội',
N'T2 - T6',
3,
N'2 năm',
N'Không yêu cầu',
'2026-12-31'),

-- JOB 2
(2,
N'Xây dựng giao diện web hiện đại bằng ReactJS.

- Phát triển UI/UX
- Tối ưu hiệu năng',

N'- Biết ReactJS, JavaScript
- Hiểu HTML/CSS',

N'- Thưởng dự án + KPI
- Môi trường startup năng động
- Free snack, coffee',

N'456 Lê Văn Sỹ, Quận 3, TP.HCM',
N'T2 - T6',
2,
N'1 năm',
N'Không yêu cầu',
'2026-11-30'),

-- JOB 3
(3,
N'Thiết kế giao diện web/app chuyên nghiệp.

- Thiết kế UI/UX
- Làm việc với dev team',

N'- Biết Figma, Photoshop
- Có gu thẩm mỹ tốt',

N'- Môi trường sáng tạo
- Thưởng theo sản phẩm
- Đào tạo nâng cao',

N'78 Nguyễn Văn Linh, Hải Châu, Đà Nẵng',
N'Linh hoạt',
1,
N'1 năm',
N'Không yêu cầu',
'2026-10-15'),

-- JOB 4
(4,
N'Quản lý sổ sách kế toán, lập báo cáo tài chính.

- Theo dõi thu chi
- Lập báo cáo định kỳ',

N'- Biết Excel
- Hiểu nghiệp vụ kế toán',

N'- Lương ổn định
- Đầy đủ chế độ BHXH
- Công việc lâu dài',

N'22 Trần Duy Hưng, Cầu Giấy, Hà Nội',
N'T2 - T7',
2,
N'2 năm',
N'Nữ',
'2026-09-30'),

-- JOB 5
(5,
N'Triển khai chiến dịch marketing online.

- Chạy quảng cáo
- SEO và content',

N'- Biết Facebook Ads, Google Ads
- Kỹ năng viết content',

N'- Thưởng KPI cao
- Cơ hội thăng tiến
- Du lịch công ty',

N'99 Nguyễn Huệ, Quận 1, TP.HCM',
N'T2 - T6',
2,
N'1 năm',
N'Không yêu cầu',
'2026-08-31');