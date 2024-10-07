CREATE TABLE Department (
    id SERIAL PRIMARY KEY,
    name VARCHAR(255) NOT NULL
);

CREATE TABLE Specialization (
    id SERIAL PRIMARY KEY,
    name VARCHAR(255) NOT NULL
);

CREATE TABLE Term (
    id SERIAL PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    start_date DATE NOT NULL,
    end_date DATE NOT NULL
);

CREATE TABLE Lecturer (
    id SERIAL PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    department_id INT NOT NULL,
    total_hours INT,
    hours_teaching INT,
    hours_research INT,
    hours_others INT,
    rank VARCHAR(50),
    FOREIGN KEY (department_id) REFERENCES Department(id)
    ON DELETE SET NULL
    ON UPDATE CASCADE
);

CREATE TABLE Subject (
    id SERIAL PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    specialization_id INT NOT NULL,
    credits INT NOT NULL,
    is_mandatory BOOLEAN NOT NULL,
    FOREIGN KEY (specialization_id) REFERENCES Specialization(id)
    ON DELETE CASCADE
    ON UPDATE CASCADE
);

CREATE TABLE LecturerSubject (
    lecturer_id INT NOT NULL,
    subject_id INT NOT NULL,
    hours_per_subject INT NOT NULL,
    term_id INT NOT NULL,
    PRIMARY KEY (lecturer_id, subject_id),
    FOREIGN KEY (lecturer_id) REFERENCES Lecturer(id)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
    FOREIGN KEY (subject_id) REFERENCES Subject(id)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
    FOREIGN KEY (term_id) REFERENCES Term(id)
    ON DELETE CASCADE
    ON UPDATE CASCADE
);

CREATE TABLE StudentGroup (
    id SERIAL PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    student_count INT NOT NULL,
    specialization_id INT NOT NULL,
    term_start DATE NOT NULL,
    term_end DATE NOT NULL,
    FOREIGN KEY (specialization_id) REFERENCES Specialization(id)
    ON DELETE CASCADE
    ON UPDATE CASCADE
);

CREATE TABLE Classroom (
    id SERIAL PRIMARY KEY,
    building_address VARCHAR(255) NOT NULL,
    room_number VARCHAR(50) NOT NULL,
    seating_capacity INT NOT NULL,
    availability VARCHAR(255) NOT NULL,
    department_id INT NOT NULL,
    FOREIGN KEY (department_id) REFERENCES Department(id)
    ON DELETE SET NULL
    ON UPDATE CASCADE
);

CREATE TABLE Equipment (
    id SERIAL PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    classroom_id INT NOT NULL,
    condition VARCHAR(50),
    FOREIGN KEY (classroom_id) REFERENCES Classroom(id)
    ON DELETE CASCADE
    ON UPDATE CASCADE
);

CREATE TABLE ClassType (
    id SERIAL PRIMARY KEY,
    type_name VARCHAR(50) NOT NULL,
    description VARCHAR(255)
);

CREATE TABLE Schedule (
    id SERIAL PRIMARY KEY,
    student_group_id INT NOT NULL,
    lesson_number INT NOT NULL,
    lesson_time_start TIME NOT NULL,
    lesson_time_end TIME NOT NULL,
    week_pattern VARCHAR(50),
    lecturer_id INT NOT NULL,
    classroom_id INT NOT NULL,
    class_type_id INT NOT NULL,
    subject_id INT NOT NULL,
    FOREIGN KEY (student_group_id) REFERENCES StudentGroup(id)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
    FOREIGN KEY (lecturer_id) REFERENCES Lecturer(id)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
    FOREIGN KEY (classroom_id) REFERENCES Classroom(id)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
    FOREIGN KEY (class_type_id) REFERENCES ClassType(id)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
    FOREIGN KEY (subject_id) REFERENCES Subject(id)
    ON DELETE CASCADE
    ON UPDATE CASCADE
);
