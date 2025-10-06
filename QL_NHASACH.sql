CREATE DATABASE QL_NHASACH_1
USE QL_NHASACH_1


-- Bảng Nhà Sản Xuất
CREATE TABLE tblNhaSanXuat (
    MaNSX VARCHAR (30) PRIMARY KEY NOT NULL,
    TenNSX NVARCHAR(100) 
);

-- Bảng Loại
CREATE TABLE tblLoai (
    MaLoai VARCHAR (30) PRIMARY KEY  NOT NULL,
    TenLoai NVARCHAR(100)
);

-- Bảng Sản Phẩm
CREATE TABLE tblSanPham (
    MaSP VARCHAR (50) PRIMARY KEY  NOT NULL,
    TenSP NVARCHAR(100),
    MaLoai VARCHAR (30),
    MaNSX VARCHAR (30),
    Gia DECIMAL(4,2),
    GhiChu NVARCHAR(200),
    Hinh NVARCHAR(200),
    CONSTRAINT FK_SanPham_Loai FOREIGN KEY (MaLoai) REFERENCES tblLoai(MaLoai),
    CONSTRAINT FK_SanPham_NSX FOREIGN KEY (MaNSX) REFERENCES tblNhaSanXuat(MaNSX)
);

ALTER TABLE tblSanPham
ALTER COLUMN Gia DECIMAL(18,2);



-- Bảng Khách Hàng
CREATE TABLE tblKhachHang (
    MaKH VARCHAR (50) PRIMARY KEY NOT NULL,
    TenKhachHang NVARCHAR(100),
    SoDienThoai VARCHAR(15),
    MatKhau NVARCHAR(50)
);

-- Bảng Hóa Đơn
CREATE TABLE tblHoaDon (
    MaHD VARCHAR (100) PRIMARY KEY NOT NULL,
    NgayTao DATE,
    MaKH VARCHAR (50),
    CONSTRAINT FK_HoaDon_KhachHang FOREIGN KEY (MaKH) REFERENCES tblKhachHang(MaKH)
);


-- Bảng Chi Tiết Hóa Đơn
CREATE TABLE tblChiTiet (
    MaHD VARCHAR (100) NOT NULL,
    MaSP VARCHAR (50) NOT NULL,
    SoLuong DECIMAL (4,2),
    CONSTRAINT PK_ChiTiet PRIMARY KEY (MaHD, MaSP),
    CONSTRAINT FK_ChiTiet_HoaDon FOREIGN KEY (MaHD) REFERENCES tblHoaDon(MaHD),
    CONSTRAINT FK_ChiTiet_SanPham FOREIGN KEY (MaSP) REFERENCES tblSanPham(MaSP)
);
-- Dữ liệu Nhà Xuất Bản
INSERT INTO tblNhaSanXuat (MaNSX, TenNSX) VALUES
('XB01', N'NXB Kim Đồng'),
('XB02', N'NXB Trẻ'),
('XB03', N'NXB Giáo Dục'),
('XB04', N'NXB Văn Học'),
('XB05', N'NXB Lao Động');

-- Dữ liệu Loại Sách
INSERT INTO tblLoai (MaLoai, TenLoai) VALUES
('L001', N'Truyện tranh'),
('L002', N'Tiểu thuyết'),
('L003', N'Giáo khoa'),
('L004', N'Tham khảo'),
('L005', N'Kỹ năng sống');

-- Dữ liệu Sách
INSERT INTO tblSanPham (MaSP, TenSP, MaLoai, MaNSX, Gia, GhiChu, Hinh) VALUES
('SP001', N'Doraemon Tập 1', 'L001','XB01', 25000, N'Truyện tranh thiếu nhi', N'dore.jpg'),
('SP002', N'Harry Potter và Hòn đá phù thủy','L002','XB02', 30000, N'Tiểu thuyết phiêu lưu', N'harry.jpg'),
('SP003', N'Sách Giáo Khoa Toán 10','L003','XB03', 250000, N'Sách học sinh cấp 3', N'toan10.jpg'),
('SP004', N'Từ Điển Anh - Việt','L004','XB04', 267000, N'Tài liệu tham khảo', N'tudien.jpg'),
('SP005', N'7 Thói quen thành đạt','L005','XB05', 200000, N'Sách kỹ năng sống', N'7thoiquen.jpg');

-- Dữ liệu Khách Hàng
INSERT INTO tblKhachHang (MaKH, TenKhachHang, SoDienThoai, MatKhau) VALUES
('KH01', N'Nguyễn Văn An', '0901234567', N'123456'),
('KH02', N'Trần Thị Bình', '0912345678', N'abcdef'),
('KH03', N'Lê Văn Cường', '0923456789', N'qwerty'),
('KH04', N'Phạm Thị Dung', '0934567890', N'pass123'),
('KH05', N'Huỳnh Văn Em', '0945678901', N'123abc');


SET DATEFORMAT 'DMY'
-- Dữ liệu Hóa Đơn
INSERT INTO tblHoaDon (MaHD, NgayTao, MaKH) VALUES
('HD001', '01-10-2025', 'KH01'),
('HD002', '03-04-2025', 'KH02'),
('HD003', '28-09-2025', 'KH03'),
('HD004', '05-02-2025', 'KH04'),
('HD005', '20-02-2025', 'KH05');




-- Dữ liệu Chi Tiết Hóa Đơn
INSERT INTO tblChiTiet (MaHD, MaSP, SoLuong) VALUES
('HD001','SP001', 10),
('HD002','SP002', 5), 
('HD003','SP003', 7), 
('HD004','SP004', 2), 
('HD005','SP005', 1); 
