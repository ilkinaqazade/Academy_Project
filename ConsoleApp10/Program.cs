using System;
using System.Linq;
using System.Net.Mail;
using System.Net;

namespace ConsoleApp10
{
    internal class Program
    {
        public class Academy
        {
            public int AcademyId { get; set; }
            public string AcademyName { get; set; }
            public Group[] AllGroup { get; set; }
            public Teacher[] AllTeachers { get; set; }

            public Academy()
            {
                AllGroup = new Group[0];
                AllTeachers = new Teacher[0];
            }

            public void AddGroup(Group newGroup, Teacher groupTeacher)
            {
                if (AllGroup == null)
                {
                    AllGroup = new Group[1];
                }
                else
                {
                    Group[] newGroupList = new Group[AllGroup.Length + 1];
                    Array.Copy(AllGroup, newGroupList, AllGroup.Length);
                    AllGroup = newGroupList;
                }

                AllGroup[AllGroup.Length - 1] = newGroup;

                if (groupTeacher != null)
                {
                    AddTeacher(groupTeacher);
                }
            }

            public void AddTeacher(Teacher newTeacher)
            {
                if (AllTeachers == null)
                {
                    AllTeachers = new Teacher[1];
                }
                else
                {
                    Teacher[] newTeacherList = new Teacher[AllTeachers.Length + 1];
                    Array.Copy(AllTeachers, newTeacherList, AllTeachers.Length);
                    AllTeachers = newTeacherList;
                }

                AllTeachers[AllTeachers.Length - 1] = newTeacher;
            }

            public Student FindStudentById(int studentId)
            {
                foreach (var group in AllGroup)
                {
                    foreach (var student in group.GroupStudentList)
                    {
                        if (student.StudentId == studentId)
                        {
                            return student;
                        }
                    }
                }
                return null;
            }

            public void ShowAcademy()
            {
                Console.WriteLine("****** Academy Info ******");
                Console.WriteLine($"Academy Id: {AcademyId}");
                Console.WriteLine($"Academy Name: {AcademyName}");
                Console.WriteLine("\nAll Groups:");

                if (AllGroup != null && AllGroup.Length > 0)
                {
                    foreach (var group in AllGroup)
                    {
                        Console.WriteLine($"Group Name: {group.GroupName}");
                        group.ShowGroup();
                    }
                }
                else
                {
                    Console.WriteLine("No groups available.");
                }
            }

            public void ShowStudentAverages(int studentId)
            {
                bool studentExists = false;

                foreach (var group in AllGroup)
                {
                    foreach (var student in group.GroupStudentList)
                    {
                        if (student.StudentId == studentId)
                        {
                            studentExists = true;
                            Console.WriteLine("****** Student Info ******");
                            Console.WriteLine($"Group: {group.GroupName}");
                            Console.WriteLine($"Student ID: {student.StudentId}");
                            Console.WriteLine($"Student Name: {student.StudentName}");
                            Console.WriteLine($"Student Email: {student.StudentEmail}");
                            Console.WriteLine("Student Averages:");

                            if (student.StudentAverage != null && student.StudentAverage.Length > 0)
                            {
                                foreach (var average in student.StudentAverage)
                                {
                                    Console.WriteLine($"  Average: [{average}]");
                                }
                            }
                            else
                            {
                                Console.WriteLine("  No averages available.");
                            }
                        }
                    }

                    if (studentExists)
                    {
                        break;
                    }
                }

                if (!studentExists)
                {
                    Console.WriteLine("Student not found.");
                }
            }
        }

        public class Group
        {
            public int GroupId { get; set; }
            public string GroupName { get; set; }
            public Student[] GroupStudentList { get; set; }

            public Group(string groupName)
            {
                GroupName = groupName;
            }

            public void AddNewGroup(Student newStudent, Teacher newTeacher)
            {
                if (GroupStudentList == null)
                {
                    GroupStudentList = new Student[1];
                }
                else
                {
                    Student[] newGroupList = new Student[GroupStudentList.Length + 1];
                    Array.Copy(GroupStudentList, newGroupList, GroupStudentList.Length);
                    GroupStudentList = newGroupList;
                }

                GroupStudentList[GroupStudentList.Length - 1] = newStudent;
                newStudent.StudentId = GroupStudentList.Length;
            }

            public void ShowGroup()
            {
                Console.WriteLine();
                Console.WriteLine("****** Group Info ******");
                Console.WriteLine($"Group Id: {GroupId}");
                Console.WriteLine($"Group Name: {GroupName}");
                Console.WriteLine("Group Students:");

                if (GroupStudentList != null)
                {
                    foreach (var student in GroupStudentList)
                    {
                        student.ShowStudent();
                    }
                }
                else
                {
                    Console.WriteLine("No students available.");
                }
            }
        }

        public class Teacher
        {
            public Teacher(string teacherName, int teacherAge)
            {
                TeacherId++;
                TeacherName = teacherName;
                TeacherAge = teacherAge;
            }

            public int TeacherId { get; set; }
            public string TeacherName { get; set; }
            public int TeacherAge { get; set; }

            public void AddStudentAverage(Student student, double score)
            {
                if (student != null)
                {
                    if (student.StudentAverage == null)
                    {
                        student.StudentAverage = new double[1];
                    }
                    else
                    {
                        double[] newAverageList = new double[student.StudentAverage.Length + 1];
                        Array.Copy(student.StudentAverage, newAverageList, student.StudentAverage.Length);
                        student.StudentAverage = newAverageList;
                    }

                    student.StudentAverage[student.StudentAverage.Length - 1] = score;
                }
            }
        }

        public class Student
        {
            public Student(string studentName, byte studentAge, string studentEmail)
            {
                StudentId++;
                StudentName = studentName;
                StudentAge = studentAge;
                StudentEmail = studentEmail;
            }

            public int StudentId { get; set; }
            public string StudentName { get; set; } 
            public byte StudentAge { get; set; } 
            public double[] StudentAverage { get; set; }
            public string StudentEmail { get; set; } 

            public void ShowStudent()
            {
                Console.WriteLine();
                Console.WriteLine("****** Student Info ******");
                Console.WriteLine($"  Student Id: {StudentId}");
                Console.WriteLine($"  Student Name: {StudentName}");
                Console.WriteLine($"  Student Age: {StudentAge}");
                Console.WriteLine($"  Student Email: {StudentEmail}");
                Console.WriteLine("  Student Averages:");

                if (StudentAverage != null)
                {
                    foreach (var average in StudentAverage)
                    {
                        Console.WriteLine($"    Average: [{average}]");
                    }
                }
                else
                {
                    Console.WriteLine("    No averages available.");
                }
            }

            public void SendEmail(double score)
            {
                try
                {
                    string smtpServer = "smtp.gmail.com";
                    int smtpPort = 587;
                    string smtpUsername = "aqazadeilkin187@gmail.com";
                    string smtpPassword = "xjhdv3cv3j24q523w52hgzupwdde4oof";

                    MailMessage mail = new MailMessage();
                    SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort)
                    {
                        Credentials = new NetworkCredential(smtpUsername, smtpPassword),
                        EnableSsl = true,
                    };

                    mail.From = new MailAddress(smtpUsername);
                    mail.To.Add(StudentEmail);
                    mail.Body = $"Avarage add => {score} ";
                    mail.Subject = "Avarage";
                    smtpClient.Send(mail);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("E-post Error: " + ex.Message);
                }
            }
        }

        static void Main(string[] args)
        {
            Academy myAcademy = new Academy
            {
                AcademyId = 1,
                AcademyName = "My Academy"
            };

            Teacher newTeacher = new Teacher("Elvin", 22);

            myAcademy.AddTeacher(newTeacher);

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("--------Select Login--------");
                Console.WriteLine("[1] Teacher");
                Console.WriteLine("[2] Student");
                Console.WriteLine("[3] Exit");
                Console.Write("Select [1] [2] [3] => ");
                int selectLogin = int.Parse(Console.ReadLine());

                if (selectLogin == 1)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("Select Teacher Name: ");
                    string checkName = Console.ReadLine();
                    var checkLogin = myAcademy.AllTeachers.SingleOrDefault(s => s.TeacherName == checkName);
                    if (checkLogin != null)
                    {
                        while (true)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\n****** Menu ******");
                            Console.WriteLine("[1] Add Group (and Teacher/Student)");
                            Console.WriteLine("[2] Show Academy");
                            Console.WriteLine("[3] Add Student Average");
                            Console.WriteLine("[4] Back to Login");
                            Console.Write("Select: ");
                            string choice = Console.ReadLine();

                            switch (choice)
                            {
                                case "1":
                                    do
                                    {
                                        Console.Clear();
                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.Write("Enter Group Name: ");
                                        string groupName = Console.ReadLine();
                                        Group newGroup = new Group(groupName);

                                        Console.Clear();
                                        Console.Write("Enter Teacher Name: ");
                                        string teacherName = Console.ReadLine();
                                        Console.Write("Enter Teacher Age: ");
                                        int teacherAge = int.Parse(Console.ReadLine());
                                        Teacher groupTeacher = new Teacher(teacherName, teacherAge);

                                        Console.Write("Do you want to add a student to this group? (yes/no): ");
                                        string studentAddGroup = Console.ReadLine().ToLower();

                                        while (studentAddGroup == "yes")
                                        {
                                            Console.Clear();
                                            Console.Write("Enter Student Name: ");
                                            string studentName = Console.ReadLine();
                                            Console.Write("Enter Student Age: ");
                                            byte studentAge = byte.Parse(Console.ReadLine());
                                            Console.Write("Enter Student Email: ");
                                            string studentEmail = Console.ReadLine();
                                            Student newStudent = new Student(studentName, studentAge, studentEmail);
                                            newGroup.AddNewGroup(newStudent, groupTeacher);

                                            Console.WriteLine("Do you want to add another student to this group? (yes/no): ");
                                            studentAddGroup = Console.ReadLine().ToLower();
                                        }

                                        myAcademy.AddGroup(newGroup, groupTeacher);
                                        Console.Clear();
                                        Console.WriteLine("Group, Teacher, and Student added successfully!");

                                        Console.Write("Do you want to add another group? (yes/no): ");
                                    } while (Console.ReadLine().ToLower() == "yes");
                                    break;

                                case "2":
                                    Console.Clear();
                                    myAcademy.ShowAcademy();
                                    break;

                                case "3":
                                    Console.Clear();
                                    myAcademy.ShowAcademy();
                                    Console.Write("Enter Student ID: ");
                                    int studentIdToAddAverage = int.Parse(Console.ReadLine());

                                    var studentToAddAverage = myAcademy.FindStudentById(studentIdToAddAverage);

                                    if (studentToAddAverage != null)
                                    {
                                        Console.Write("Enter Student Average: ");
                                        double studentAverage = double.Parse(Console.ReadLine());

                                        newTeacher.AddStudentAverage(studentToAddAverage, studentAverage);
                                        studentToAddAverage.SendEmail(studentAverage); 

                                        Console.WriteLine("Average added successfully!");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Student not found.");
                                    }
                                    break;

                                case "4":
                                    break;

                                default:
                                    Console.WriteLine("Invalid choice. Please try again.");
                                    break;
                            }

                            if (choice == "4")
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("The entered name was not found in the system");
                    }
                }
                else if (selectLogin == 2)
                {
                    while (true)
                    {
                        Console.WriteLine("Menu:");
                        Console.WriteLine("[1] Show Average");
                        Console.WriteLine("[2] Back to Login");
                        Console.Write("Enter your choice: ");
                        string choice = Console.ReadLine();

                        switch (choice)
                        {
                            case "1":
                                Console.Clear();
                                Console.Write("Enter Student ID: ");
                                int studentId = int.Parse(Console.ReadLine());
                                myAcademy.ShowStudentAverages(studentId);
                                break;

                            case "2":
                                break;

                            default:
                                Console.WriteLine("Invalid choice. Please try again.");
                                break;
                        }

                        if (choice == "2")
                        {
                            break;
                        }
                    }
                }
                else if (selectLogin == 3)
                {
                    Console.WriteLine("Exiting the program.");
                    Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine("Invalid choice. Please try again.");
                }
            }
        }
    }
}
