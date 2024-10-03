-- Tạo cơ sở dữ liệu
CREATE DATABASE dottie;
USE dottie;

-- Tạo bảng Users (thông tin người dùng)
CREATE TABLE Users (
    userId INT PRIMARY KEY IDENTITY(1,1), 
    username NVARCHAR(255) NOT NULL UNIQUE,
    email NVARCHAR(255) NOT NULL UNIQUE,
    password NVARCHAR(255) NOT NULL,
    role NVARCHAR(50) DEFAULT 'khách hàng' 
);

-- Tạo bảng Categories (danh mục sản phẩm)
CREATE TABLE Categories (
    categoryId INT PRIMARY KEY IDENTITY(1,1), 
    categoryName NVARCHAR(255) NOT NULL
);

-- Tạo bảng Products (sản phẩm)
CREATE TABLE Products (
    productId INT PRIMARY KEY IDENTITY(1,1), 
    productName NVARCHAR(255) NOT NULL,
    price DECIMAL(10, 2) NOT NULL,
    description NVARCHAR(MAX),
    stock INT DEFAULT 0,
    categoryId INT,
    imageUrl NVARCHAR(255),
    discount DECIMAL(5, 2) default 0,
    FOREIGN KEY (categoryId) REFERENCES Categories(categoryId)
);

-- Tạo bảng Orders (đơn hàng)
CREATE TABLE Orders (
    orderId INT PRIMARY KEY IDENTITY(1,1), 
    userId INT,
    totalAmount DECIMAL(10, 2) NOT NULL,
    orderDate DATETIME DEFAULT GETDATE(),
    status NVARCHAR(50) DEFAULT 'Pending',
    shippingAddress NVARCHAR(255),
    phoneNumber NVARCHAR(20),
    FOREIGN KEY (userId) REFERENCES Users(userId)
);

-- Tạo bảng OrderDetails (chi tiết đơn hàng)
CREATE TABLE OrderDetails (
    orderDetailId INT PRIMARY KEY IDENTITY(1,1),
    orderId INT,
    productId INT,
    quantity INT NOT NULL,
    price DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (orderId) REFERENCES Orders(orderId),
    FOREIGN KEY (productId) REFERENCES Products(productId)
);

-- Tạo bảng ShoppingCart (giỏ hàng)
CREATE TABLE ShoppingCart (
    cartId INT PRIMARY KEY IDENTITY(1,1), 
    userId INT,
    createdAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (userId) REFERENCES Users(userId)
);

-- Tạo bảng CartItems (chi tiết giỏ hàng)
CREATE TABLE CartItems (
    cartItemId INT PRIMARY KEY IDENTITY(1,1),
    cartId INT,
    productId INT,
    quantity INT NOT NULL,
    FOREIGN KEY (cartId) REFERENCES ShoppingCart(cartId),
    FOREIGN KEY (productId) REFERENCES Products(productId)
);

-- Tạo bảng Payments (thanh toán)
CREATE TABLE Payments (
    paymentId INT PRIMARY KEY IDENTITY(1,1), 
    orderId INT,
    paymentMethod NVARCHAR(50),
    paymentStatus NVARCHAR(50) DEFAULT 'Pending',
    paymentDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (orderId) REFERENCES Orders(orderId)
);

-- Tạo bảng Favorites (sản phẩm yêu thích)
CREATE TABLE Favorites (
    favoriteId INT PRIMARY KEY IDENTITY(1,1),
    userId INT,
    createdAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (userId) REFERENCES Users(userId)
);

-- Tạo bảng FavoriteItems (chi tiết sản phẩm yêu thích)
CREATE TABLE FavoriteItems (
    favoriteItemId INT PRIMARY KEY IDENTITY(1,1),
    favoriteId INT,
    productId INT,
    FOREIGN KEY (favoriteId) REFERENCES Favorites(favoriteId),
    FOREIGN KEY (productId) REFERENCES Products(productId)
);


-- Chèn dữ liệu mẫu vào bảng Users
INSERT INTO Users (username, email, password, role) VALUES 
(N'admin', N'maithaituan129@gmail.com', N'091204', N'Admin'),
(N'user1', N'user1@example.com', N'password1', N'Khách hàng'),
(N'user2', N'user2@example.com', N'password2', N'Khách hàng');

-- Chèn dữ liệu mẫu vào bảng Categories
INSERT INTO Categories (categoryName) VALUES 
(N'Trousers'),
(N'T-Shirts'),
(N'Jackets');

-- Chèn dữ liệu mẫu vào bảng Products
INSERT INTO Products (productName, price, description, stock, categoryId, imageUrl, discount) VALUES 
(N'Jeans', 50.00, N'Comfortable denim jeans', 20, 1, N'jeans.jpg', 15.00), 
(N'T-Shirt', 20.00, N'Soft cotton T-shirt', 50, 2, N'tshirt.jpg', NULL),
(N'Jacket', 80.00, N'Warm winter jacket', 15, 3, N'jacket.jpg', 10.00);

-- Chèn dữ liệu mẫu vào bảng Orders
INSERT INTO Orders (userId, totalAmount, orderDate, status, shippingAddress, phoneNumber) VALUES 
(1, 75.00, GETDATE(), N'Pending', N'123 Street, City', N'1234567890'),
(2, 25.00, GETDATE(), N'Pending', N'456 Avenue, City', N'0987654321');

-- Chèn dữ liệu mẫu vào bảng OrderDetails
INSERT INTO OrderDetails (orderId, productId, quantity, price) VALUES 
(1, 1, 1, 50.00), 
(1, 2, 1, 25.00),
(2, 3, 1, 25.00);  

-- Chèn dữ liệu mẫu vào bảng ShoppingCart
INSERT INTO ShoppingCart (userId) VALUES 
(1), 
(2);

-- Chèn dữ liệu mẫu vào bảng CartItems
INSERT INTO CartItems (cartId, productId, quantity) VALUES 
(1, 1, 1), 
(1, 2, 2), 
(2, 3, 1); 

-- Chèn dữ liệu mẫu vào bảng Payments
INSERT INTO Payments (orderId, paymentMethod, paymentStatus) VALUES 
(1, N'Credit Card', N'Pending'),
(2, N'Debit Card', N'Pending');

-- Chèn dữ liệu mẫu vào bảng Favorites
INSERT INTO Favorites (userId) VALUES 
(1), 
(2);

-- Chèn dữ liệu mẫu vào bảng FavoriteItems
INSERT INTO FavoriteItems (favoriteId, productId) VALUES 
(1, 1),  -- Sản phẩm yêu thích của userId 1
(1, 2),  -- Sản phẩm yêu thích khác của userId 1
(2, 3);  -- Sản phẩm yêu thích của userId 2

