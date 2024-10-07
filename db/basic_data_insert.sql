-- Insert data into Department
INSERT INTO Department (name) VALUES
('Programming'),
('Cyber security'),
('Computational mathematics');

-- Insert data into Specialization
INSERT INTO Specialization (name) VALUES
('Computer Science'),
('Applied Mathematics'),
('Cyber Security'),
('System Analysis');

-- Insert data into Term
INSERT INTO Term (name, start_date, end_date) VALUES
('Fall 2024', '2024-09-01', '2024-11-30'),
('Spring 2025', '2025-01-15', '2025-05-15');

-- Insert data into Lecturer
INSERT INTO Lecturer (name, department_id, total_hours, hours_teaching, hours_research, hours_others, rank) VALUES
('Sviatoslav Litynskii', 1, 598, 525, 57, 16, 'Docent'),
('Taras Zabolotskii', 1, 593, 451, 60, 82, 'Professor');

-- Insert data into Subject
INSERT INTO Subject (name, specialization_id, credits, is_mandatory) VALUES
('Information and Encoding Theory', 1, 6, TRUE),
('Computer Systems Architecture', 1, 6, TRUE),
('Data Bases and Informational Systems', 1, 6, TRUE);

-- Insert data into LecturerSubject
INSERT INTO LecturerSubject (lecturer_id, subject_id, hours_per_subject, term_id) VALUES
(2, 1, 52, 2),
(2, 2, 32, 2),
(1, 3, 70, 2);

-- Insert data into StudentGroup
INSERT INTO StudentGroup (name, student_count, specialization_id, term_start, term_end) VALUES
('PMI-31', 21, 1, '2024-09-01', '2025-05-15');

-- Insert data into Classroom
INSERT INTO Classroom (building_address, room_number, seating_capacity, availability, department_id) VALUES
('Universytetska st., 1', '270', 25, 'Available', 1),
('Universytetska st., 1', '119a', 25, 'Available', 1);

-- Insert data into Equipment
INSERT INTO Equipment (name, classroom_id, condition) VALUES
('Projector', 1, 'Good'),
('Whiteboard', 1, 'Good'),
('Computers', 1, 'Good'),
('Projector', 2, 'Bad'),
('Whiteboard', 2, 'Good'),
('Computers', 2, 'Good');

-- Insert data into ClassType
INSERT INTO ClassType (type_name, description) VALUES
('Lecture', 'A formal teaching session'),
('Lab', 'A hands-on practical session');

