--Create Database 
IF NOT EXISTS (SELECT * FROM sys.sysdatabases WHERE name='QuanLyNhaSach')
	BEGIN
	   CREATE DATABASE [QuanLyNhaSach]
	END
GO
USE [QuanLyNhaSach]
GO

create table PhanQuyen
(
	ID int IDENTITY(1,1) PRIMARY KEY,
	TenPhanQuyen nvarchar(50),
)

create table TaiKhoan
(
	IDTaiKhoan int IDENTITY(1,1) PRIMARY KEY,
	TenTaiKhoan nvarchar(50) NOT NULL,
	MatKhau nvarchar(50) NOT NULL,
	Email nvarchar(50) NOT NULL,
	SDT int,
	NgaySinh date,
	GioiTinh bit,
	ID int, 
	visible int,

	FOREIGN KEY (ID) REFERENCES PhanQuyen(ID), 
)

create table KhachHang
(
	IDKhachHang int IDENTITY(1,1) PRIMARY KEY,
	TenKhachHang nvarchar(50),
	SDT int,
	Email nvarchar(50),
	NgaySinh date,
	SoTienDaMua int,
	CapBac nvarchar(50),
	GioiTinh bit,
	visible int
)


create table HoaDon
(
	IDHoaDon int IDENTITY(1,1) PRIMARY KEY,
	NgayInHoaDon date,
	GioInHoaDon time,
	TongTien int,
	IDTaiKhoan int,
	IDKhachHang int,
	FOREIGN KEY (IDTaiKhoan) REFERENCES TaiKhoan(IDTaiKhoan),
	FOREIGN KEY (IDKhachHang) REFERENCES KhachHang(IDKhachHang),
)


create table Sach
(
	IDSach int IDENTITY(1,1) PRIMARY KEY,
	TenSach nvarchar(50) NOT NULL,
	NXB nvarchar(50) NOT NULL,
	TenTacGia nvarchar(50) NOT NULL,
	Gia float(2),
	SoLuong int NOT NULL,
	visible int
)

create table ChiTietHoaDon
(
	IDChiTietHoaDon int IDENTITY(1,1) PRIMARY KEY,
	SoLuong int,
	IDHoaDon int,
	IDSach int,
	FOREIGN KEY (IDHoaDon) REFERENCES HoaDon(IDHoaDon),
	FOREIGN KEY (IDSach) REFERENCES Sach(IDSach),
)

create table LuuID
(
	IDChiTietHoaDon int IDENTITY(1,1) PRIMARY KEY,
	IDStaff int,
)

--Insert Data
use QuanLyNhaSach
Go
--Trigger
--Cap nhat so tien da mua
CREATE TRIGGER UpdateSoTienDaMua
ON HoaDon
AFTER INSERT
AS
BEGIN
    DECLARE @IDKhachHang int
    DECLARE @TongTien int

    -- Lấy thông tin IDKhachHang và TongTien từ bản ghi vừa thêm vào
    SELECT @IDKhachHang = inserted.IDKhachHang, @TongTien = inserted.TongTien
    FROM inserted

    -- Cập nhật Sotiendamua của KhachHang
    UPDATE KhachHang
    SET SoTienDaMua = SoTienDaMua + @TongTien
    WHERE IDKhachHang = @IDKhachHang
END
--Update cap bac
CREATE TRIGGER UpdateCustomerLevel
ON HoaDon
AFTER INSERT,UPDATE
AS
BEGIN
    DECLARE @CustomerId INT;
    DECLARE @TotalAmount INT;

    SELECT @CustomerId = i.IDKhachHang, @TotalAmount = SUM(s.Gia)
    FROM inserted i
    JOIN ChiTietHoaDon c ON i.IDHoaDon = c.IDHoaDon
    JOIN Sach s ON c.IDSach = s.IDSach
    WHERE i.IDKhachHang IS NOT NULL AND s.Gia > 0
    GROUP BY i.IDKhachHang;

    IF @CustomerId IS NOT NULL
    BEGIN
        DECLARE @CustomerTotalAmount INT;
        DECLARE @CustomerLevel NVARCHAR(50);

        SELECT @CustomerTotalAmount = SUM(TongTien), @CustomerLevel = KhachHang.CapBac
        FROM HoaDon
        JOIN KhachHang ON HoaDon.IDKhachHang = KhachHang.IDKhachHang
        WHERE KhachHang.IDKhachHang = @CustomerId AND TongTien > 0
        GROUP BY KhachHang.CapBac;

        IF @CustomerTotalAmount IS NULL
            SET @CustomerTotalAmount = 0;

        IF @CustomerLevel IS NULL
            SET @CustomerLevel = 'Thành viên';

        IF @CustomerTotalAmount >= 1000000 AND @CustomerLevel <> 'Thành viên thân thiết'
        BEGIN
            UPDATE KhachHang
            SET CapBac = 'Thành viên thân thiết'
            WHERE IDKhachHang = @CustomerId;
        END
        ELSE IF @CustomerTotalAmount >= 5000000 AND @CustomerLevel <> 'VIP'
        BEGIN
            UPDATE KhachHang
            SET CapBac = 'VIP'
            WHERE IDKhachHang = @CustomerId;
        END
    END
END


insert into LuuID values(1)
--D? li?u b?ng Phân Quy?n
insert into PhanQuyen values(N'Quan Ly')
insert into PhanQuyen values(N'Nhan Vien')

-- Thêm d? li?u vào b?ng TaiKhoan

INSERT INTO TaiKhoan (TenTaiKhoan, MatKhau, Email, SDT, NgaySinh, GioiTinh, ID, visible)
VALUES
    (N'Tran Thi Minh Anh', 'password123', 'minhanh@gmail.com', 123456789, '2002-05-15', 1, 1, 1),
    (N'Nguyen Tien Anh', 'abc123', 'nguyenanh@gmail.com', 987654321, '2003-08-20', 0, 2, 1),
    (N'Le Thi Bao', 'pass456', 'lebao@gmail.com', 555555555, '1999-02-10', 1, 2, 1),
    (N'Pham Van Cuong', 'pass789', 'phamcuong@gmail.com', 666666666, '1995-12-05', 0, 2, 1),
    (N'Hoang Thi Dao', 'qwe123', 'hoangdao@gmail.com', 777777777, '1998-03-25', 1, 2, 0),
    (N'Truong Van Thanh', 'zxc456', 'truongthanh@gmail.com', 888888888, '2000-06-18', 0, 2, 0),
    (N'Ngo Van Hai', 'asd789', 'ngohai@gmail.com', 999999999, '1993-09-08', 1, 1, 1),
    (N'Mai Thi Giang', 'poi123', 'maigiang@gmail.com', 111111111, '1994-11-30', 0, 2, 1),
    (N'Phan Van Khai', 'mnb456', 'phankhai@gmail.com', 222222222, '1992-07-12', 1, 2, 1),
    (N'Dang Thi Ngoc', 'lkj789', 'dangngoc@gmail.com', 333333333, '1997-02-22', 0, 2, 0),
    (N'Vu Van Lam', 'yui123', 'vulam@gmail.com', 444444444, '1991-04-02', 1, 1, 1),
    (N'Duong Thi Thao', 'tgb456', 'duongthao@gmail.com', 555555555, '1996-10-14', 0, 1, 0),
    (N'Bui Thi Ngan', 'edc789', 'buingan@gmail.com', 666666666, '1998-08-28', 1, 1, 0),
    (N'Ly Van Quy', 'rfv123', 'lyquy@gmail.com', 777777777, '1999-01-01', 0, 2, 1),
    (N'Trinh Thi Huong', 'wsx456', 'trinhhuong@gmail.com', 888888888, '1992-03-11', 1, 2, 1),
    (N'Vo Thi Quynh', 'ujm789', 'voquynh@gmail.com', 999999999, '2001-07-07', 0, 2, 1),
    (N'Ha Van Son', 'zax123', 'havanson@gmail.com', 111111111, '1984-06-24', 1, 1, 0),
    (N'Nguyen Van Tam', 'plm456', 'nguyentam@gmail.com', 222222222, '1996-12-19', 0, 2, 0),
    (N'Le Thi Tuyet', 'oki789', 'letuyet@gmail.com', 333333333, '1987-05-05', 1, 1, 1),
    (N'Tran Van Tuan', 'wer123', 'trantuan@gmail.com', 444444444, '1999-02-14', 0, 2, 1);


-- Thêm dữ liệu vào bảng KhachHang
INSERT INTO KhachHang (TenKhachHang, SDT, Email, NgaySinh, SoTienDaMua, CapBac, GioiTinh)
VALUES
    (N'Trần Văn Minh', 123456789, 'minhanh@gmail.com', '1990-05-15', 5000000, N'VIP', 1),
    (N'Nguyễn Thị Bình', 987654321, 'nguyenb@gmail.com', '1995-08-20', 2000000, N'Thường', 0),
    (N'Lê Thành Công', 555555555, 'lethic@gmail.com', '1988-02-10', 10000000, N'VIP', 1),
    (N'Phạm Thị Đông', 666666666, 'phamd@gmail.com', '1992-12-05', 7500000, N'VIP', 0),
    (N'Hoàng Văn Huy', 777777777, 'hoange@gmail.com', '1987-03-25', 3000000, N'Thường', 1),
    (N'Trường Thị Lý', 888888888, 'truongf@gmail.com', '1998-06-18', 4000000, N'Thường', 0),
    (N'Ngô Đình Hải', 999999999, 'ngog@gmail.com', '1991-09-08', 6000000, N'VIP', 1),
    (N'Mai Thị Thảo', 111111111, 'maih@gmail.com', '1994-11-30', 9000000, N'VIP', 0),
    (N'Phan Van Khôi', 222222222, 'phani@gmail.com', '1986-07-12', 2500000, N'Thường', 1),
    (N'Đặng Thị Linh', 333333333, 'dangk@gmail.com', '1997-02-22', 4000000, N'Thường', 0),
    (N'Vũ Minh Lâm', 444444444, 'vul@gmail.com', '1993-04-02', 7000000, N'VIP', 1),
    (N'Dương Thanh Mỹ', 555555555, 'duongm@gmail.com', '1989-10-14', 6000000, N'VIP', 0),
    (N'Bùi Thị Ngọc Trâm', 666666666, 'buin@gmail.com', '1996-08-28', 3500000, N'Thường', 1),
    (N'Lý Văn Phương', 777777777, 'lyp@gmail.com', '1990-01-01', 2000000, N'Thường', 0),
    (N'Trịnh Văn Quân', 888888888, 'trinhq@gmail.com', '1985-03-11', 8000000, N'VIP', 1),
    (N'Võ Thị Quỳnh Nga', 999999999, 'vor@gmail.com', '1999-07-07', 4500000, N'Thường', 0),
    (N'Hà Văn Sơn', 111111111, 'has@gmail.com', '1984-06-24', 5500000, N'VIP', 1),
    (N'Nguyễn Văn Tâm', 222222222, 'nguyent@gmail.com', '1993-12-19', 6500000, N'VIP', 0),
    (N'Lê Thị Thúy', 333333333, 'leu@gmail.com', '1987-05-05', 3000000, N'Thường', 1),
    (N'Trần Văn Tuấn', 444444444, 'tranv@gmail.com', '1995-02-14', 4000000, N'Thường', 0);

--Insert dữ liệu hóa đơn
INSERT INTO HoaDon (NgayInHoaDon, GioInHoaDon, TongTien, IDTaiKhoan, IDKhachHang)
VALUES
    ('2023-09-10', '10:30:00', 1500000, 3, 1),
    ('2023-09-09', '15:45:00', 800000, 4, 2),
    ('2023-09-08', '14:20:00', 3000000, 5, 3),
    ('2023-09-07', '12:15:00', 2500000, 6, 4),
    ('2023-09-06', '09:00:00', 4000000, 7, 5),
    ('2023-09-05', '17:30:00', 1800000, 8, 6),
    ('2023-09-04', '11:55:00', 2200000, 9, 7),
    ('2023-09-03', '14:10:00', 3500000, 10, 8),
    ('2023-09-02', '13:20:00', 2800000, 11, 9),
    ('2023-09-01', '16:40:00', 4200000, 12, 10),
    ('2023-08-31', '10:00:00', 1950000, 13, 11),
    ('2023-08-30', '08:15:00', 3600000, 14, 12),
    ('2023-08-29', '19:30:00', 3100000, 15, 13),
    ('2023-08-28', '14:05:00', 2750000, 16, 14),
    ('2023-08-27', '11:45:00', 4200000, 17, 15),
    ('2023-08-26', '18:20:00', 1750000, 18, 16),
    ('2023-08-25', '10:10:00', 2250000, 19, 17),
    ('2023-08-24', '16:25:00', 3100000, 20, 18),
    ('2023-08-23', '15:00:00', 2600000, 21, 19),
    ('2023-08-22', '12:50:00', 4000000, 22, 20),
    ('2023-08-21', '14:30:00', 2100000, 3, 1),
    ('2023-08-20', '09:40:00', 3300000, 4, 2),
    ('2023-08-19', '17:15:00', 1750000, 5, 3),
    ('2023-08-18', '13:05:00', 2900000, 6, 4),
    ('2023-08-17', '15:50:00', 2450000, 7, 5);
---- Thêm dữ liệu vào bảng Sach
INSERT INTO Sach (TenSach, NXB, TenTacGia, Gia, SoLuong)
VALUES
    ('Cong Nghe Thong Tin', 'Nha Xuat Ban Kim Dong', 'Nguyen Van Anh', 150000, 50),
    ('Lich Su Viet Nam', 'Nha Xuat Ban Van Hoc', 'Tran Thi Minh', 200000, 30),
    ('Hoa Hoc Co Ban', 'Nha Xuat Ban Khoa Hoc', 'Le Thanh Cong', 180000, 40),
    ('Toan Cao Cap', 'Nha Xuat Ban Giao Duc', 'Pham Dinh Dong', 220000, 20),
    ('Van Hoc Nhat Ban', 'Nha Xuat Ban Ngoai Van', 'Hoang Minh Huy', 250000, 25),
    ('Kinh Te Hoc', 'Nha Xuat Ban Kinh Te', 'Truong Thi Lan', 160000, 35),
    ('Lap Trinh Python', 'Nha Xuat Ban Cong Nghe', 'Nguyen Van Quan', 190000, 45),
    ('So Dia Ly The Gioi', 'Nha Xuat Ban Dai Hoc', 'Mai Thi Thao', 210000, 15),
    ('Tieu Thuyet Hai Huoc', 'Nha Xuat Ban Hai Kich', 'Phan Dinh Cuong', 230000, 10),
    ('Tam Ly Hoc', 'Nha Xuat Ban Tam Ly', 'Dang Thi Huong', 170000, 30),
    ('Ky Thuat Lap Trinh', 'Nha Xuat Ban Cong Nghe', 'Vu Dinh Lam', 260000, 28),
    ('Sach Thieu Nhi', 'Nha Xuat Ban Thieu Nhi', 'Duong Van My', 290000, 22),
    ('Truyen Ngan', 'Nha Xuat Ban Van Hoc', 'Bui Thi Ngoc', 140000, 48),
    ('Quan Tri Kinh Doanh', 'Nha Xuat Ban Kinh Doanh', 'Ly Van Phuong', 180000, 40),
    ('Lich Su The Gioi', 'Nha Xuat Ban Ngoai Van', 'Trinh Quynh Nga', 320000, 12),
    ('Ngon Ngu Hoc', 'Nha Xuat Ban Ngon Ngu', 'Vo Thi Quynh', 280000, 18),
    ('Hoa Hoc Huu Co', 'Nha Xuat Ban Khoa Hoc', 'Ha Van Son', 260000, 22),
    ('Sach Tu Hoc Tieng Anh', 'Nha Xuat Ban Ngoai Ngu', 'Nguyen Van Tam', 240000, 24),
    ('Van Hoc Viet Nam', 'Nha Xuat Ban Van Hoc', 'Le Thi Thuy', 300000, 16),
    ('Kinh Te The Gioi', 'Nha Xuat Ban Kinh Te', 'Tran Van Tuan', 270000, 19),
    ('Lap Trinh Java', 'Nha Xuat Ban Cong Nghe', 'Nguyen Van Hung', 200000, 25),
    ('Dai Cuong Hoa Hoc', 'Nha Xuat Ban Khoa Hoc', 'Pham Thi Hoa', 180000, 30),
    ('Toan Hoc Dai Cuong', 'Nha Xuat Ban Giao Duc', 'Tran Van Binh', 220000, 28),
    ('Van Hoc Phap', 'Nha Xuat Ban Ngoai Van', 'Le Thi Dieu', 260000, 18),
    ('Lich Su Dong Nam A', 'Nha Xuat Ban Van Hoc', 'Phan Van Thanh', 240000, 20),
    ('Kinh Te Hoc Phat Trien', 'Nha Xuat Ban Kinh Te', 'Trinh Thi Lan', 280000, 22),
    ('Lap Trinh C++', 'Nha Xuat Ban Cong Nghe', 'Le Minh Hoa', 190000, 24),
    ('Vat Ly Hoc Dai Cuong', 'Nha Xuat Ban Khoa Hoc', 'Do Van Dung', 210000, 32),
    ('Chinh Tri Hoc', 'Nha Xuat Ban Dai Hoc', 'Ly Thi Huong', 170000, 26),
    ('Sach Ky Nang Song', 'Nha Xuat Ban Tam Ly', 'Nguyen Thi Hai', 230000, 22),
    ('Lap Trinh Ruby', 'Nha Xuat Ban Cong Nghe', 'Tran Van Tuan', 200000, 27),
    ('Sach Tieu Thuyet Ngon Tinh', 'Nha Xuat Ban Van Hoc', 'Tran Thi Lan Anh', 270000, 16),
    ('Kinh Te Hoc Vi Mo', 'Nha Xuat Ban Kinh Te', 'Pham Van Duc', 230000, 25);

	--Chi tiet hoa don
INSERT INTO ChiTietHoaDon (SoLuong, IDHoaDon, IDSach)
VALUES
    (5, 1, 39),
    (3, 2, 40),
    (4, 3, 41),
    (2, 4, 42),
    (6, 5, 43),
    (7, 6, 44),
    (8, 7, 45),
    (3, 8, 46),
    (5, 9, 47),
    (4, 10, 48),
    (2, 11, 49),
    (6, 12, 50),
    (7, 13, 51),
    (8, 14, 52),
    (3, 15, 53),
    (5, 16, 54),
    (4, 17, 55),
    (2, 18, 56),
    (6, 19, 57),
    (7, 20, 58),
    (8, 21, 59),
    (3, 22, 60),
    (5, 23, 61),
    (4, 24, 62),
    (2, 25, 63);



-- Thêm d? li?u vào b?ng ChiTietHoaDon
INSERT INTO ChiTietHoaDon (SoLuong, IDHoaDon, IDSach)
VALUES
    (2, 4, 1),
    (3, 5, 2),
    (1, 6, 3),
    (4, 2, 4),
    (2, 7, 5),
    (3, 8, 6),
    (2, 9, 7),
    (1, 10, 8),
    (3, 11, 9),
    (2, 12, 10),
    (1, 13, 11),
    (4, 14, 12),
    (2, 15, 13),
    (3, 16, 14),
    (1, 17, 15),
    (4, 18, 16),
    (2, 19, 17),
    (3, 20, 18),
    (1, 21, 19),
    (2,22, 20);

	select *
	from HoaDon

-- Thêm d? li?u vào b?ng LuuID
INSERT INTO LuuID (IDStaff)
VALUES
    (1),
    (2),
    (3),
    (4),
    (5),
    (6),
    (7),
    (8),
    (9),
    (10),
    (11),
    (12),
    (13),
    (14),
    (15),
    (16),
    (17),
    (18),
    (19),
    (20);

UPDATE Sach
SET visible = 1
WHERE IDSach BETWEEN 39 AND 71;
UPDATE KhachHang
SET visible = 1
WHERE IDKhachHang BETWEEN 1 AND 20;
