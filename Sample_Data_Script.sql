-- Sample Data for Arksoft Customer Management System
-- This script inserts sample customer data for testing and demonstration

-- Insert sample customers
INSERT INTO [Customers] ([Name], [Address], [TelephoneNumber], [ContactPersonName], [ContactPersonEmail], [VatNumber], [CreatedAt], [UpdatedAt])
VALUES 
    ('Acme Corporation', '123 Business Park, Johannesburg, 2001', '+27 11 123 4567', 'John Smith', 'john.smith@acme.com', 'ZA1234567890', GETDATE(), NULL),
    ('TechSolutions Ltd', '456 Technology Street, Cape Town, 8001', '+27 21 987 6543', 'Sarah Johnson', 'sarah@techsolutions.co.za', 'ZA9876543210', GETDATE(), NULL),
    ('Global Industries', '789 Industrial Avenue, Durban, 4001', '+27 31 555 1234', 'Michael Brown', 'mbrown@globalind.com', 'ZA5555666677', GETDATE(), NULL),
    ('StartUp Innovations', '321 Innovation Hub, Pretoria, 0001', '+27 12 777 8899', 'Emma Davis', 'emma.davis@startup.co.za', NULL, GETDATE(), NULL),
    ('Premier Services', '654 Service Road, Port Elizabeth, 6001', '+27 41 444 5555', 'David Wilson', 'dwilson@premier.co.za', 'ZA1111222233', GETDATE(), NULL),
    ('Local Business Co', '987 Main Street, Bloemfontein, 9301', NULL, 'Lisa Taylor', 'lisa@localbusiness.co.za', NULL, GETDATE(), NULL),
    ('Enterprise Holdings', '147 Corporate Plaza, East London, 5201', '+27 43 666 7777', 'Robert Anderson', 'robert@enterprise.co.za', 'ZA8888999900', GETDATE(), NULL),
    ('Small Business Ltd', '258 Small Business Park, Nelspruit, 1200', '+27 13 222 3333', NULL, NULL, 'ZA3333444455', GETDATE(), NULL),
    ('Consulting Group', '369 Professional Centre, Polokwane, 0700', '+27 15 111 2222', 'Jennifer White', 'jwhite@consulting.co.za', NULL, GETDATE(), NULL),
    ('Manufacturing Co', '741 Factory Road, Kimberley, 8300', '+27 53 999 0000', 'Mark Thompson', 'mthompson@manufacturing.co.za', 'ZA6666777788', GETDATE(), NULL),
    ('Retail Solutions', '852 Shopping Centre, George, 6530', '+27 44 333 4444', 'Amanda Clark', 'amanda@retail.co.za', 'ZA2222333344', GETDATE(), NULL),
    ('Professional Services', '963 Business District, Rustenburg, 0300', '+27 14 555 6666', 'Christopher Lee', 'clee@professional.co.za', NULL, GETDATE(), NULL);

PRINT 'Sample customer data has been successfully inserted.';
PRINT 'Total customers inserted: 12';