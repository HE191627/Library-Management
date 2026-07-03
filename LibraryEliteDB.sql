-- =============================================
-- PROJECT: LIBRARY ELITE DATABASE
-- AUTHOR: VẠN KIẾM QUYẾT
-- =============================================

USE master;
GO
IF EXISTS (SELECT name FROM sys.databases WHERE name = N'LibraryEliteDB')
    DROP DATABASE LibraryEliteDB;
GO
CREATE DATABASE LibraryEliteDB;
GO
USE LibraryEliteDB;
GO

-- 1. TABLE: CATEGORIES
CREATE TABLE Categories (
    CategoryId INT PRIMARY KEY IDENTITY(1,1),
    CategoryName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255)
);

-- 2. TABLE: AUTHORS
CREATE TABLE Authors (
    AuthorId INT PRIMARY KEY IDENTITY(1,1),
    AuthorName NVARCHAR(150) NOT NULL,
    Biography NVARCHAR(MAX)
);

-- 3. TABLE: BOOKS
CREATE TABLE Books (
    BookId INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(200) NOT NULL,
    ISBN VARCHAR(20) UNIQUE,
    AuthorId INT FOREIGN KEY REFERENCES Authors(AuthorId),
    CategoryId INT FOREIGN KEY REFERENCES Categories(CategoryId),
    PublishYear INT,
    Price DECIMAL(18, 2),
    Quantity INT DEFAULT 0,
    Summary NVARCHAR(MAX),
    CreatedAt DATETIME DEFAULT GETDATE(),
    Status NVARCHAR(50) DEFAULT 'Available'
);

-- 4. TABLE: MEMBERS
CREATE TABLE Members (
    MemberId INT PRIMARY KEY IDENTITY(1,1),
    FullName NVARCHAR(100) NOT NULL,
    Email VARCHAR(100) UNIQUE,
    Phone VARCHAR(20),
    Address NVARCHAR(255),
    JoinDate DATE DEFAULT GETDATE(),
    Status NVARCHAR(50) DEFAULT 'Active'
);

-- 5. TABLE: LOANS
CREATE TABLE Loans (
    LoanId INT PRIMARY KEY IDENTITY(1,1),
    MemberId INT FOREIGN KEY REFERENCES Members(MemberId),
    BorrowDate DATETIME DEFAULT GETDATE(),
    DueDate DATETIME NOT NULL,
    ReturnDate DATETIME NULL,
    Status NVARCHAR(50) DEFAULT 'Borrowed',
    Notes NVARCHAR(MAX)
);

-- 6. TABLE: LOAN DETAILS
CREATE TABLE LoanDetails (
    DetailId INT PRIMARY KEY IDENTITY(1,1),
    LoanId INT FOREIGN KEY REFERENCES Loans(LoanId),
    BookId INT FOREIGN KEY REFERENCES Books(BookId),
    Quantity INT DEFAULT 1
);

CREATE TABLE Accounts (
    AccountId INT PRIMARY KEY IDENTITY(1,1),
    Username VARCHAR(50) UNIQUE NOT NULL,
    Password NVARCHAR(255) NOT NULL,
    Role NVARCHAR(50) DEFAULT 'Member', -- Admin hoặc Member
    MemberId INT FOREIGN KEY REFERENCES Members(MemberId) ON DELETE CASCADE,
    IsActive BIT DEFAULT 1
);

-- =============================================
-- SEED DATA (BƠM DỮ LIỆU CỰC DÀY)
-- =============================================

-- INSERT CATEGORIES
INSERT INTO Categories (CategoryName, Description) VALUES 
(N'Lập trình phần mềm', N'C#, Java, Python, Web, Mobile'),
(N'Cơ sở dữ liệu', N'SQL Server, NoSQL, Data Design'),
(N'Trí tuệ nhân tạo', N'AI, Machine Learning, Data Science'),
(N'Kỹ năng mềm', N'Giao tiếp, lãnh đạo, quản lý thời gian'),
(N'Kinh doanh & Khởi nghiệp', N'Kinh tế, Quản trị, Marketing'),
(N'Tâm lý học', N'Tâm lý hành vi, tư duy'),
(N'Văn học & Nghệ thuật', N'Tiểu thuyết, truyện ngắn, thơ'),
(N'Khoa học viễn tưởng', N'Công nghệ tương lai, vũ trụ');

-- INSERT AUTHORS
INSERT INTO Authors (AuthorName, Biography) VALUES 
(N'Robert C. Martin', N'Clean Code Expert'), (N'Andrew Hunt', N'Pragmatic Programmer'),
(N'Donny Tia', N'SQL Expert'), (N'Dale Carnegie', N'How to Win Friends'),
(N'Simon Sinek', N'Start with Why'), (N'Yuval Noah Harari', N'Sapiens Author'),
(N'J.K. Rowling', N'Harry Potter Creator'), (N'Steve McConnell', N'Code Complete Author'),
(N'Martin Fowler', N'Refactoring Guru'), (N'Eric Evans', N'Domain Driven Design'),
(N'Napoleon Hill', N'Think and Grow Rich'), (N'George Orwell', N'1984 Author');

-- INSERT BOOKS (50+ BOOKS)
INSERT INTO Books (Title, ISBN, AuthorId, CategoryId, PublishYear, Price, Quantity, Summary) VALUES 
-- IT Sector
(N'Clean Code', '9780132350884', 1, 1, 2008, 450000, 10, N'Mã sạch và tư duy lập trình viên.'),
(N'The Pragmatic Programmer', '9780201616224', 2, 1, 1999, 380000, 5, N'Lập trình thực chiến.'),
(N'SQL Execution Plans', '9781906404628', 3, 2, 2012, 250000, 15, N'Tối ưu SQL.'),
(N'Refactoring', '9780134757599', 9, 1, 2018, 550000, 12, N'Tái cấu trúc mã nguồn.'),
(N'Code Complete 2', '9780735619678', 8, 1, 2004, 480000, 8, N'Xây dựng phần mềm chất lượng.'),
(N'Domain-Driven Design', '9780321125217', 10, 1, 2003, 620000, 4, N'Thiết kế hướng tên miền.'),
(N'C# 10 in a Nutshell', '9781098100964', 1, 1, 2022, 750000, 20, N'Toàn tập về C# hiện đại.'),
(N'Design Patterns', '9780201633610', 1, 1, 1994, 400000, 7, N'23 mẫu thiết kế kinh điển.'),
(N'Head First Java', '9781491776', 2, 1, 2021, 320000, 10, N'Học Java cực vui.'),
(N'Deep Learning', '9780262035613', 1, 3, 2016, 890000, 6, N'Kiến thức chuyên sâu AI.'),
-- Business & Soft Skills
(N'Đắc Nhân Tâm', '9786045880623', 4, 4, 1936, 85000, 50, N'Nghệ thuật giao tiếp.'),
(N'Start with Why', '9781591846444', 5, 5, 2009, 180000, 25, N'Bắt đầu với câu hỏi Tại sao.'),
(N'Leaders Eat Last', '9781591845324', 5, 5, 2014, 220000, 18, N'Lãnh đạo phục vụ.'),
(N'Think and Grow Rich', '9781593302009', 11, 5, 1937, 150000, 30, N'Nghĩ giàu và làm giàu.'),
(N'Sapiens', '9780062316097', 6, 6, 2014, 250000, 40, N'Lược sử loài người.'),
(N'Homo Deus', '9781784703936', 6, 6, 2016, 280000, 30, N'Lược sử tương lai.'),
-- Literature & Fiction
(N'Harry Potter 1', 'HP001', 7, 7, 1997, 150000, 100, N'Hòn đá phù thủy.'),
(N'Harry Potter 2', 'HP002', 7, 7, 1998, 155000, 85, N'Phòng chứa bí mật.'),
(N'Harry Potter 3', 'HP003', 7, 7, 1999, 160000, 70, N'Tên tù nhân Azkaban.'),
(N'1984', '9780451524935', 12, 8, 1949, 120000, 15, N'Xã hội tương lai giả tưởng.'),
(N'Animal Farm', '9780451526342', 12, 8, 1945, 95000, 20, N'Trại súc vật.'),
(N'The Alchemist', '9780062315007', 4, 7, 1988, 110000, 45, N'Nhà giả kim.');

-- INSERT MEMBERS (15+ MEMBERS)
INSERT INTO Members (FullName, Email, Phone, Address, Status) VALUES 
(N'Nguyễn Văn A', 'vana@gmail.com', '0912345678', N'Hà Nội', 'Active'),
(N'Trần Thị B', 'thib@gmail.com', '0988776655', N'TP.HCM', 'Active'),
(N'Lê Văn Luyện', 'luyenlv@fpt.edu.vn', '0901223344', N'Thanh Xuân, Hà Nội', 'Active'),
(N'Nguyễn Thị Nở', 'no.nguyen@yahoo.com', '0944556677', N'Hà Nam', 'Locked'),
(N'Chí Phèo', 'pheoc@gmail.com', '0988000111', N'Vũ Đại', 'Active'),
(N'Trương Gia Bình', 'binhtg@fpt.com.vn', '0900112233', N'Cầu Giấy, Hà Nội', 'Active'),
(N'Bill Gates', 'bill@microsoft.com', '0123', N'Washington', 'Active'),
(N'Elon Musk', 'elon@tesla.com', '0999', N'Texas', 'Active'),
(N'Phạm Nhật Vượng', 'vuongpn@vingroup.com', '0888', N'Hà Nội', 'Active'),
(N'Sơn Tùng MTP', 'tungmtp@sky.com', '0777', N'Thái Bình', 'Active');

-- INSERT SOME LOANS FOR TEST
INSERT INTO Loans (MemberId, BorrowDate, DueDate, Status) VALUES 
(1, GETDATE()-5, GETDATE()+2, 'Borrowed'),
(2, GETDATE()-10, GETDATE()-3, 'Overdue'),
(3, GETDATE()-2, GETDATE()+5, 'Borrowed');

INSERT INTO LoanDetails (LoanId, BookId, Quantity) VALUES 
(1, 1, 1), (1, 3, 1), (2, 5, 1), (3, 10, 1);

INSERT INTO Accounts (Username, Password, Role, MemberId) VALUES 
('admin', '123', 'Admin', 1),        -- Bạn là Admin
('vana', '123', 'Member', 2),
('thib', '123', 'Member', 3),
('luyenlv', '123', 'Member', 4),
('no.nguyen', '123', 'Member', 5),
('pheoc', '123', 'Member', 6),
('binhtg', 'admin123', 'Member', 7),      
('bill', '123', 'Member', 8),
('elon', '123', 'Member', 9),
('vuongpn', 'admin123', 'Member', 10),   
('tungmtp', '123', 'Member', 11);