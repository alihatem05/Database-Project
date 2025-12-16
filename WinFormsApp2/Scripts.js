use Agent-Activities-Tracker

db.createCollection("clients");
db.createCollection("employees");
db.createCollection("cases");
db.createCollection("actions");

db.clients.insertMany([
    {
        client_id: "C001",
        first_name: "Omar",
        last_name: "Khaled",
        phone_number: "01012345678",
        date_of_birth: new Date("1998-03-21"),
        sex: "M",
        email: "omar.khaled@gmail.com",
        primary_address: {
            street: "12 Nadi El Seid",
            city: "Giza",
            country: "Egypt"
        }
    },
    {
        client_id: "C002",
        first_name: "Sara",
        last_name: "Mostafa",
        phone_number: "01098765432",
        date_of_birth: new Date("1995-11-10"),
        sex: "F",
        email: "sara.mostafa@yahoo.com",
        primary_address: {
            street: "7 Abbas El Akkad",
            city: "Nasr City",
            country: "Egypt"
        }
    },
    {
        client_id: "C003",
        first_name: "Hassan",
        last_name: "Saeed",
        phone_number: "01122334455",
        date_of_birth: new Date("1992-07-15"),
        sex: "M",
        email: "hassan.saeed@ymail.com",
        primary_address: {
            street: "3 El Tahrir",
            city: "Cairo",
            country: "Egypt"
        }
    },
    {
        client_id: "C004",
        first_name: "Mona",
        last_name: "Fahmy",
        phone_number: "01233445566",
        date_of_birth: new Date("1990-12-01"),
        sex: "F",
        email: "mona.fahmy@gmail.com",
        primary_address: {
            street: "25 Nile St",
            city: "Alexandria",
            country: "Egypt"
        }
    },
    {
        client_id: "C005",
        first_name: "Ali",
        last_name: "Hatem",
        phone_number: "01044556677",
        date_of_birth: new Date("1988-04-20"),
        sex: "M",
        email: "ali.hatem@outlook.com",
        primary_address: {
            street: "18 Taha Hussein",
            city: "Cairo",
            country: "Egypt"
        }
    },
    {
        client_id: "C006",
        first_name: "Nour",
        last_name: "Ahmed",
        phone_number: "01155667788",
        date_of_birth: new Date("1997-09-05"),
        sex: "F",
        email: "nour.ahmed@yahoo.com",
        primary_address: {
            street: "5 Ramses",
            city: "Giza",
            country: "Egypt"
        }
    },
    {
        client_id: "C007",
        first_name: "Khaled",
        last_name: "Mahmoud",
        phone_number: "01266778899",
        date_of_birth: new Date("1993-02-18"),
        sex: "M",
        email: "khaled.mahmoud@gmail.com",
        primary_address: {
            street: "11 El Geish",
            city: "Cairo",
            country: "Egypt"
        }
    },
    {
        client_id: "C008",
        first_name: "Laila",
        last_name: "Hassan",
        phone_number: "01077889900",
        date_of_birth: new Date("1996-06-12"),
        sex: "F",
        email: "laila.hassan@ymail.com",
        primary_address: {
            street: "8 Tahrir Square",
            city: "Cairo",
            country: "Egypt"
        }
    },
    {
        client_id: "C009",
        first_name: "Tamer",
        last_name: "Adel",
        phone_number: "01188990011",
        date_of_birth: new Date("1994-10-30"),
        sex: "M",
        email: "tamer.adel@gmail.com",
        primary_address: {
            street: "14 El Nasr",
            city: "Alexandria",
            country: "Egypt"
        }
    },
    {
        client_id: "C010",
        first_name: "Dina",
        last_name: "Sami",
        phone_number: "01299001122",
        date_of_birth: new Date("1991-05-25"),
        sex: "F",
        email: "dina.sami@outlook.com",
        primary_address: {
            street: "20 Cleopatra",
            city: "Alexandria",
            country: "Egypt"
        }
    },
    {
        client_id: "C011",
        first_name: "Youssef",
        last_name: "Wael",
        phone_number: "01282934472",
        date_of_birth: new Date("2006-06-12T17:34:09.889Z"),
        sex: "Male",
        email: "youssef2006126@gmail.com",
        primary_address: {
            street: "Maadi",
            city: "Cairo",
            country: "Egypt"
        }
    },
    {
        client_id: "C012",
        first_name: "Farah",
        last_name: "Tarek",
        phone_number: "01054398232",
        date_of_birth: new Date("2005-12-14T18:12:12.342Z"),
        sex: "F",
        email: "farah.tarek@gmail.com",
        primary_address: {
            street: "Alb",
            city: "el gehaz el dawry",
            country: "Ali Hatem"
        }
    }
]);

db.employees.insertMany([
    {
        employee_id: "E001",
        first_name: "Mina",
        last_name: "Farouk",
        phone_number: "01010001111",
        email: "mina.farouk@gmail.com",
        role: "supervisor",
        password: "Mina2025"
    },
    {
        employee_id: "E002",
        first_name: "Nada",
        last_name: "Sherif",
        phone_number: "01120002222",
        email: "nada.sherif@outlook.com",
        role: "supervisor",
        password: "NadaStar88"
    },
    {
        employee_id: "E003",
        first_name: "Adel",
        last_name: "Naguib",
        phone_number: "01230003333",
        email: "adel.naguib@yahoo.com",
        role: "agent",
        agent_info: { supervisor_id: "E001" },
        password: "Adel007X"
    },
    {
        employee_id: "E004",
        first_name: "Reem",
        last_name: "Ibrahim",
        phone_number: "01040004444",
        email: "reem.ibrahim@ymail.com",
        role: "agent",
        agent_info: { supervisor_id: "E001" },
        password: "ReemSun42"
    },
    {
        employee_id: "E005",
        first_name: "Kareem",
        last_name: "Ragab",
        phone_number: "01150005555",
        email: "kareem.ragab@gmail.com",
        role: "agent",
        agent_info: { supervisor_id: "E001" },
        password: "KareemR99"
    },
    {
        employee_id: "E006",
        first_name: "Salma",
        last_name: "Yousry",
        phone_number: "01260006666",
        email: "salma.yousry@outlook.com",
        role: "agent",
        agent_info: { supervisor_id: "E001" },
        password: "SalmaSky21"
    },
    {
        employee_id: "E007",
        first_name: "Yassin",
        last_name: "Mahmoud",
        phone_number: "01070007777",
        email: "yassin.mahmoud@yahoo.com",
        role: "agent",
        agent_info: { supervisor_id: "E002" },
        password: "YassinMoon7"
    },
    {
        employee_id: "E008",
        first_name: "Hana",
        last_name: "Zaki",
        phone_number: "01180008888",
        email: "hana.zaki@ymail.com",
        role: "agent",
        agent_info: { supervisor_id: "E002" },
        password: "HanaBlue88"
    },
    {
        employee_id: "E009",
        first_name: "Othman",
        last_name: "Fathy",
        phone_number: "01290009999",
        email: "othman.fathy@gmail.com",
        role: "agent",
        agent_info: { supervisor_id: "E002" },
        password: "Othman99X"
    },
    {
        employee_id: "E010",
        first_name: "Lina",
        last_name: "Galal",
        phone_number: "01010101010",
        email: "lina.galal@outlook.com",
        role: "agent",
        agent_info: { supervisor_id: "E002" },
        password: "LinaSunset1"
    }
]);

db.cases.insertMany([
    {
        case_id: "K001",
        title: "Internet Connectivity Issue",
        description: "Client reports frequent internet disconnection.",
        priority: "high",
        status: "closed",
        case_origin: "phone",
        creation_date: new Date("2025-01-01"),
        tags: ["internet", "router", "disconnect"],
        client_id: "C001",
        agent_id: "E003",
        supervisor_id: "E002",
        actions: ["A001", "A002", "A003", "A004", "A005", "A051", "A052"],
        closed_date: new Date("2025-12-05T02:56:45.242Z")
    },
    {
        case_id: "K002",
        title: "Billing Error",
        description: "Incorrect amount in latest invoice.",
        priority: "medium",
        status: "closed",
        case_origin: "email",
        creation_date: new Date("2025-01-05"),
        tags: ["billing", "payment"],
        client_id: "C002",
        agent_id: "E004",
        supervisor_id: "E001",
        actions: ["A006", "A007", "A008", "A009", "A010"],
        closed_date: new Date("2025-01-10")
    },
    {
        case_id: "K003",
        title: "Slow Internet Speed",
        description: "Client complains about slow internet.",
        priority: "medium",
        status: "closed",
        case_origin: "phone",
        creation_date: new Date("2025-01-03"),
        tags: ["internet", "speed"],
        client_id: "C003",
        agent_id: "E005",
        supervisor_id: "E002",
        actions: ["A011", "A012", "A013", "A014", "A015", "A053", "A054"],
        closed_date: new Date("2025-12-05T15:50:56.429Z")
    },
    {
        case_id: "K004",
        title: "Router Malfunction",
        description: "Router not connecting to ISP.",
        priority: "high",
        status: "closed",
        case_origin: "in-person",
        creation_date: new Date("2025-01-07"),
        tags: ["router", "hardware"],
        client_id: "C004",
        agent_id: "E006",
        supervisor_id: "E002",
        actions: ["A016", "A017", "A018", "A019", "A020"],
        closed_date: new Date("2025-01-12")
    },
    {
        case_id: "K005",
        title: "Email Configuration Issue",
        description: "Client cannot send emails.",
        priority: "low",
        status: "closed",
        case_origin: "email",
        creation_date: new Date("2025-01-09"),
        tags: ["email", "configuration"],
        client_id: "C005",
        agent_id: "E007",
        supervisor_id: "E001",
        actions: ["A021", "A022", "A023", "A024", "A025"],
        closed_date: new Date("2025-12-05T02:41:23.695Z")
    },
    {
        case_id: "K006",
        title: "VPN Connection Problem",
        description: "VPN disconnects frequently.",
        priority: "medium",
        status: "closed",
        case_origin: "phone",
        creation_date: new Date("2025-01-10"),
        tags: ["vpn", "network"],
        client_id: "C006",
        agent_id: "E008",
        supervisor_id: "E002",
        actions: ["A026", "A027", "A028", "A029", "A030"],
        closed_date: new Date("2025-01-15")
    },
    {
        case_id: "K007",
        title: "Software Installation Issue",
        description: "Client cannot install required software.",
        priority: "low",
        status: "open",
        case_origin: "email",
        creation_date: new Date("2025-01-12"),
        tags: ["software", "installation"],
        client_id: "C007",
        agent_id: "E009",
        supervisor_id: "E001",
        actions: ["A031", "A032", "A033", "A034", "A035"]
    },
    {
        case_id: "K008",
        title: "Billing Refund Request",
        description: "Client requests refund for overcharge.",
        priority: "medium",
        status: "closed",
        case_origin: "phone",
        creation_date: new Date("2025-01-14"),
        tags: ["billing", "refund"],
        client_id: "C008",
        agent_id: "E010",
        supervisor_id: "E002",
        actions: ["A036", "A037", "A038", "A039", "A040"],
        closed_date: new Date("2025-01-18")
    },
    {
        case_id: "K009",
        title: "Account Access Issue",
        description: "Client cannot login to account.",
        priority: "high",
        status: "open",
        case_origin: "in-person",
        creation_date: new Date("2025-01-16"),
        tags: ["account", "login"],
        client_id: "C009",
        agent_id: "E003",
        supervisor_id: "E001",
        actions: ["A041", "A042", "A043", "A044", "A045"]
    },
    {
        case_id: "K010",
        title: "Payment Gateway Error",
        description: "Client payment fails repeatedly.",
        priority: "high",
        status: "closed",
        case_origin: "email",
        creation_date: new Date("2025-01-18"),
        tags: ["payment", "gateway"],
        client_id: "C010",
        agent_id: "E004",
        supervisor_id: "E002",
        actions: ["A046", "A047", "A048", "A049", "A050"],
        closed_date: new Date("2025-01-22")
    }
])


db.actions.insertMany([
    { action_id: "A001", action_type: "investigation", description: "Checked router logs and connection history.", timestamp: ISODate("2025-01-01T10:00:00Z"), duration_minutes: 30, is_reviewed: true, last_modified: ISODate("2025-01-01T10:30:00Z"), case_id: "K001" },
    { action_id: "A002", action_type: "phone", description: "Called client to verify problem timing.", timestamp: ISODate("2025-01-01T11:00:00Z"), duration_minutes: 10, is_reviewed: true, last_modified: ISODate("2025-01-01T11:10:00Z"), case_id: "K001" },
    { action_id: "A003", action_type: "onsite", description: "Technician visited client premises to check router.", timestamp: ISODate("2025-01-01T14:00:00Z"), duration_minutes: 45, is_reviewed: false, last_modified: ISODate("2025-01-01T14:45:00Z"), case_id: "K001" },
    { action_id: "A004", action_type: "email", description: "Sent troubleshooting steps via email.", timestamp: ISODate("2025-01-01T15:30:00Z"), duration_minutes: 15, is_reviewed: false, last_modified: ISODate("2025-01-01T15:45:00Z"), case_id: "K001" },
    { action_id: "A005", action_type: "followup", description: "Client confirmed connectivity improved.", timestamp: ISODate("2025-01-02T09:00:00Z"), duration_minutes: 10, is_reviewed: true, last_modified: ISODate("2025-01-02T09:10:00Z"), case_id: "K001" },

    { action_id: "A006", action_type: "investigation", description: "Reviewed billing system logs.", timestamp: ISODate("2025-01-05T09:00:00Z"), duration_minutes: 20, is_reviewed: true, last_modified: ISODate("2025-01-05T09:20:00Z"), case_id: "K002" },
    { action_id: "A007", action_type: "email", description: "Requested invoice details from client.", timestamp: ISODate("2025-01-05T10:00:00Z"), duration_minutes: 10, is_reviewed: true, last_modified: ISODate("2025-01-05T10:10:00Z"), case_id: "K002" },
    { action_id: "A008", action_type: "phone", description: "Confirmed billing period and charges.", timestamp: ISODate("2025-01-05T11:00:00Z"), duration_minutes: 15, is_reviewed: false, last_modified: ISODate("2025-01-05T11:15:00Z"), case_id: "K002" },
    { action_id: "A009", action_type: "adjustment", description: "Corrected the invoice and processed refund.", timestamp: ISODate("2025-01-06T14:00:00Z"), duration_minutes: 30, is_reviewed: true, last_modified: ISODate("2025-01-06T14:30:00Z"), case_id: "K002" },
    { action_id: "A010", action_type: "confirmation", description: "Client confirmed receipt of corrected invoice.", timestamp: ISODate("2025-01-07T10:00:00Z"), duration_minutes: 5, is_reviewed: true, last_modified: ISODate("2025-01-07T10:05:00Z"), case_id: "K002" },

    { action_id: "A011", action_type: "investigation", description: "Checked bandwidth usage and ISP logs.", timestamp: ISODate("2025-01-03T09:00:00Z"), duration_minutes: 25, is_reviewed: true, last_modified: ISODate("2025-01-03T09:25:00Z"), case_id: "K003" },
    { action_id: "A012", action_type: "phone", description: "Spoke with client about peak usage times.", timestamp: ISODate("2025-01-03T10:00:00Z"), duration_minutes: 10, is_reviewed: true, last_modified: ISODate("2025-01-03T10:10:00Z"), case_id: "K003" },
    { action_id: "A013", action_type: "onsite", description: "Technician checked router placement and wiring.", timestamp: ISODate("2025-01-03T14:00:00Z"), duration_minutes: 40, is_reviewed: false, last_modified: ISODate("2025-01-03T14:40:00Z"), case_id: "K003" },
    { action_id: "A014", action_type: "email", description: "Sent optimization tips to client.", timestamp: ISODate("2025-01-03T15:30:00Z"), duration_minutes: 15, is_reviewed: false, last_modified: ISODate("2025-01-03T15:45:00Z"), case_id: "K003" },
    { action_id: "A015", action_type: "followup", description: "Client confirmed speed improved.", timestamp: ISODate("2025-01-04T09:00:00Z"), duration_minutes: 5, is_reviewed: true, last_modified: ISODate("2025-01-04T09:05:00Z"), case_id: "K003" },

    { action_id: "A016", action_type: "investigation", description: "Checked router hardware and logs.", timestamp: ISODate("2025-01-07T10:00:00Z"), duration_minutes: 30, is_reviewed: true, last_modified: ISODate("2025-01-07T10:30:00Z"), case_id: "K004" },
    { action_id: "A017", action_type: "phone", description: "Contacted client for router error details.", timestamp: ISODate("2025-01-07T11:00:00Z"), duration_minutes: 15, is_reviewed: true, last_modified: ISODate("2025-01-07T11:15:00Z"), case_id: "K004" },
    { action_id: "A018", action_type: "onsite", description: "Technician replaced faulty router.", timestamp: ISODate("2025-01-07T14:00:00Z"), duration_minutes: 45, is_reviewed: false, last_modified: ISODate("2025-01-07T14:45:00Z"), case_id: "K004" },
    { action_id: "A019", action_type: "email", description: "Sent new router configuration instructions.", timestamp: ISODate("2025-01-07T15:00:00Z"), duration_minutes: 10, is_reviewed: false, last_modified: ISODate("2025-01-07T15:10:00Z"), case_id: "K004" },
    { action_id: "A020", action_type: "followup", description: "Client confirmed router is working properly.", timestamp: ISODate("2025-01-08T09:00:00Z"), duration_minutes: 5, is_reviewed: true, last_modified: ISODate("2025-01-08T09:05:00Z"), case_id: "K004" },

    { action_id: "A021", action_type: "investigation", description: "Checked client email settings.", timestamp: ISODate("2025-01-09T09:00:00Z"), duration_minutes: 20, is_reviewed: true, last_modified: ISODate("2025-01-09T09:20:00Z"), case_id: "K005" },
    { action_id: "A022", action_type: "phone", description: "Guided client through email setup.", timestamp: ISODate("2025-01-09T10:00:00Z"), duration_minutes: 15, is_reviewed: true, last_modified: ISODate("2025-01-09T10:15:00Z"), case_id: "K005" },
    { action_id: "A023", action_type: "email", description: "Sent step-by-step configuration instructions.", timestamp: ISODate("2025-01-09T11:00:00Z"), duration_minutes: 10, is_reviewed: false, last_modified: ISODate("2025-01-09T11:10:00Z"), case_id: "K005" },
    { action_id: "A024", action_type: "onsite", description: "Technician checked email client software.", timestamp: ISODate("2025-01-09T14:00:00Z"), duration_minutes: 30, is_reviewed: false, last_modified: ISODate("2025-01-09T14:30:00Z"), case_id: "K005" },
    { action_id: "A025", action_type: "followup", description: "Client confirmed email is working correctly.", timestamp: ISODate("2025-01-10T09:00:00Z"), duration_minutes: 5, is_reviewed: true, last_modified: ISODate("2025-01-10T09:05:00Z"), case_id: "K005" },

    { action_id: "A026", action_type: "investigation", description: "Checked VPN server logs and client configuration.", timestamp: ISODate("2025-01-10T10:00:00Z"), duration_minutes: 25, is_reviewed: true, last_modified: ISODate("2025-01-10T10:25:00Z"), case_id: "K006" },
    { action_id: "A027", action_type: "phone", description: "Guided client to reset VPN settings.", timestamp: ISODate("2025-01-10T11:00:00Z"), duration_minutes: 10, is_reviewed: true, last_modified: ISODate("2025-01-10T11:10:00Z"), case_id: "K006" },
    { action_id: "A028", action_type: "onsite", description: "Technician updated VPN client software.", timestamp: ISODate("2025-01-10T14:00:00Z"), duration_minutes: 30, is_reviewed: false, last_modified: ISODate("2025-01-10T14:30:00Z"), case_id: "K006" },
    { action_id: "A029", action_type: "email", description: "Sent troubleshooting guide to client.", timestamp: ISODate("2025-01-10T15:00:00Z"), duration_minutes: 15, is_reviewed: false, last_modified: ISODate("2025-01-10T15:15:00Z"), case_id: "K006" },
    { action_id: "A030", action_type: "followup", description: "Client confirmed VPN stable.", timestamp: ISODate("2025-01-11T09:00:00Z"), duration_minutes: 5, is_reviewed: true, last_modified: ISODate("2025-01-11T09:05:00Z"), case_id: "K006" },

    { action_id: "A031", action_type: "investigation", description: "Verified system requirements.", timestamp: ISODate("2025-01-12T09:00:00Z"), duration_minutes: 20, is_reviewed: true, last_modified: ISODate("2025-01-12T09:20:00Z"), case_id: "K007" },
    { action_id: "A032", action_type: "phone", description: "Guided client through installation steps.", timestamp: ISODate("2025-01-12T10:00:00Z"), duration_minutes: 15, is_reviewed: true, last_modified: ISODate("2025-01-12T10:15:00Z"), case_id: "K007" },
    { action_id: "A033", action_type: "email", description: "Sent installation guide.", timestamp: ISODate("2025-01-12T11:00:00Z"), duration_minutes: 10, is_reviewed: false, last_modified: ISODate("2025-01-12T11:10:00Z"), case_id: "K007" },
    { action_id: "A034", action_type: "onsite", description: "Technician assisted with installation.", timestamp: ISODate("2025-01-12T14:00:00Z"), duration_minutes: 30, is_reviewed: false, last_modified: ISODate("2025-01-12T14:30:00Z"), case_id: "K007" },
    { action_id: "A035", action_type: "followup", description: "Client confirmed software installed successfully.", timestamp: ISODate("2025-01-13T09:00:00Z"), duration_minutes: 5, is_reviewed: true, last_modified: ISODate("2025-01-13T09:05:00Z"), case_id: "K007" },

    { action_id: "A036", action_type: "investigation", description: "Reviewed billing and payment history.", timestamp: ISODate("2025-01-14T09:00:00Z"), duration_minutes: 20, is_reviewed: true, last_modified: ISODate("2025-01-14T09:20:00Z"), case_id: "K008" },
    { action_id: "A037", action_type: "email", description: "Requested receipt and invoice details from client.", timestamp: ISODate("2025-01-14T10:00:00Z"), duration_minutes: 10, is_reviewed: true, last_modified: ISODate("2025-01-14T10:10:00Z"), case_id: "K008" },
    { action_id: "A038", action_type: "phone", description: "Confirmed refund amount with finance.", timestamp: ISODate("2025-01-14T11:00:00Z"), duration_minutes: 15, is_reviewed: false, last_modified: ISODate("2025-01-14T11:15:00Z"), case_id: "K008" },
    { action_id: "A039", action_type: "adjustment", description: "Processed refund through billing system.", timestamp: ISODate("2025-01-15T14:00:00Z"), duration_minutes: 30, is_reviewed: true, last_modified: ISODate("2025-01-15T14:30:00Z"), case_id: "K008" },
    { action_id: "A040", action_type: "confirmation", description: "Client confirmed refund received.", timestamp: ISODate("2025-01-16T09:00:00Z"), duration_minutes: 5, is_reviewed: true, last_modified: ISODate("2025-01-16T09:05:00Z"), case_id: "K008" },

    { action_id: "A041", action_type: "investigation", description: "Checked login credentials and account status.", timestamp: ISODate("2025-01-16T09:00:00Z"), duration_minutes: 20, is_reviewed: true, last_modified: ISODate("2025-01-16T09:20:00Z"), case_id: "K009" },
    { action_id: "A042", action_type: "phone", description: "Called client to reset password.", timestamp: ISODate("2025-01-16T10:00:00Z"), duration_minutes: 10, is_reviewed: true, last_modified: ISODate("2025-01-16T10:10:00Z"), case_id: "K009" },
    { action_id: "A043", action_type: "email", description: "Sent temporary password to client.", timestamp: ISODate("2025-01-16T11:00:00Z"), duration_minutes: 10, is_reviewed: false, last_modified: ISODate("2025-01-16T11:10:00Z"), case_id: "K009" },
    { action_id: "A044", action_type: "onsite", description: "Technician checked client device.", timestamp: ISODate("2025-01-16T14:00:00Z"), duration_minutes: 30, is_reviewed: false, last_modified: ISODate("2025-01-16T14:30:00Z"), case_id: "K009" },
    { action_id: "A045", action_type: "followup", description: "Client confirmed account access restored.", timestamp: ISODate("2025-01-17T09:00:00Z"), duration_minutes: 5, is_reviewed: true, last_modified: ISODate("2025-01-17T09:05:00Z"), case_id: "K009" },

    { action_id: "A046", action_type: "investigation", description: "Checked payment gateway logs for errors.", timestamp: ISODate("2025-01-18T09:00:00Z"), duration_minutes: 25, is_reviewed: true, last_modified: ISODate("2025-01-18T09:25:00Z"), case_id: "K010" },
    { action_id: "A047", action_type: "email", description: "Sent instructions to client to retry payment.", timestamp: ISODate("2025-01-18T10:00:00Z"), duration_minutes: 10, is_reviewed: true, last_modified: ISODate("2025-01-18T10:10:00Z"), case_id: "K010" },
    { action_id: "A048", action_type: "phone", description: "Called client to verify payment details.", timestamp: ISODate("2025-01-18T11:00:00Z"), duration_minutes: 15, is_reviewed: false, last_modified: ISODate("2025-01-18T11:15:00Z"), case_id: "K010" },
    { action_id: "A049", action_type: "onsite", description: "Technician assisted client to complete transaction.", timestamp: ISODate("2025-01-18T14:00:00Z"), duration_minutes: 30, is_reviewed: false, last_modified: ISODate("2025-01-18T14:30:00Z"), case_id: "K010" },
    { action_id: "A050", action_type: "followup", description: "Client confirmed payment processed successfully.", timestamp: ISODate("2025-01-19T09:00:00Z"), duration_minutes: 5, is_reviewed: true, last_modified: ISODate("2025-01-19T09:05:00Z"), case_id: "K010" }])

db.runCommand({
    collMod: "employees",
    validator: {
        $jsonSchema: {
            bsonType: "object",
            required: ["employee_id", "first_name", "last_name", "phone_number", "email", "role"],
            properties: {
                employee_id: { bsonType: "string" },
                first_name: { bsonType: "string" },
                last_name: { bsonType: "string" },
                phone_number: {
                    bsonType: "string",
                    pattern: "^[0-9]{11}$"
                },
                email: {
                    bsonType: "string",
                    pattern: "^.+@.+\\..+$"
                },
                role: {
                    enum: ["agent", "supervisor"]
                },
                agent_info: {
                    bsonType: "object",
                    required: ["supervisor_id"],
                    properties: {
                        supervisor_id: { bsonType: "string" }
                    }
                }
            }
        }
    },
    validationLevel: "strict",
    validationAction: "error"
})

db.runCommand({
    collMod: "clients",
    validator: {
        $jsonSchema: {
            bsonType: "object",
            required: [
                "client_id",
                "first_name",
                "last_name",
                "phone_number",
                "date_of_birth",
                "sex",
                "email",
                "primary_address"
            ],
            properties: {
                client_id: { bsonType: "string" },
                first_name: { bsonType: "string" },
                last_name: { bsonType: "string" },
                phone_number: {
                    bsonType: "string",
                    pattern: "^[0-9]{11}$"
                },
                date_of_birth: { bsonType: "date" },
                sex: {
                    enum: ["M", "F"]
                },
                email: {
                    bsonType: "string",
                    pattern: "^.+@.+\\..+$"
                },
                primary_address: {
                    bsonType: "object",
                    required: ["street", "city", "country"],
                    properties: {
                        street: { bsonType: "string" },
                        city: { bsonType: "string" },
                        country: { bsonType: "string" }
                    }
                }
            }
        }
    },
    validationLevel: "strict",
    validationAction: "error"
});

db.runCommand({
    collMod: "actions",
    validator: {
        $jsonSchema: {
            bsonType: "object",
            required: [
                "action_id",
                "action_type",
                "description",
                "timestamp",
                "duration_minutes",
                "is_reviewed",
                "last_modified",
                "case_id"
            ],
            properties: {
                action_id: { bsonType: "string" },
                action_type: { bsonType: "string" },
                description: { bsonType: "string" },
                timestamp: { bsonType: "date" },
                duration_minutes: {
                    bsonType: "int",
                    minimum: 1
                },
                is_reviewed: { bsonType: "bool" },
                last_modified: { bsonType: "date" },
                case_id: { bsonType: "string" }
            }
        }
    },
    validationLevel: "strict",
    validationAction: "error"
});

db.runCommand({
    collMod: "cases",
    validator: {
        $jsonSchema: {
            bsonType: "object",
            required: [
                "case_id",
                "title",
                "priority",
                "status",
                "case_origin",
                "creation_date",
                "client_id",
                "agent_id",
                "supervisor_id"
            ],
            properties: {
                case_id: { bsonType: "string" },
                title: { bsonType: "string" },
                description: { bsonType: "string" },
                priority: {
                    enum: ["low", "medium", "high", "critical"]
                },
                status: {
                    enum: ["open", "closed"]
                },
                case_origin: {
                    enum: ["phone call", "email", "walk-in", "portal"]
                },
                creation_date: { bsonType: "date" },
                closed_date: { bsonType: "date" },
                tags: {
                    bsonType: "array",
                    items: { bsonType: "string" }
                },
                client_id: { bsonType: "string" },
                agent_id: { bsonType: "string" },
                supervisor_id: { bsonType: "string" },
                actions: {
                    bsonType: "array",
                    items: { bsonType: "string" }
                }
            }
        }
    },
    validationLevel: "strict",
    validationAction: "error"
});

db.actions.aggregate([
    { $match: { case_id: "K001" } },
    {
        $group: {
            _id: null,
            total: { $sum: 1 },
            reviewed: { $sum: { $cond: ["$is_reviewed", 1, 0] } },
            duration: { $sum: "$duration_minutes" }
        }
    }
]).toArray();

db.actions.aggregate([
    { $match: { case_id: "K001" } },
    { $sort: { timestamp: 1 } }
]).toArray();

db.clients.aggregate([
    { $sort: { client_id: -1 } },
    { $limit: 1 }
]).toArray();

db.cases.aggregate([
    { $project: { num: { $toInt: { $substr: ["$case_id", 1, -1] } } } },
    { $sort: { num: -1 } },
    { $limit: 1 }
]).toArray();

db.cases.createIndex({ agent_id: 1 }, { name: "agentCases" });
db.cases.createIndex({ supervisor_id: 1 }, { name: "supervisorCases" });

db.employees.aggregate([
    { $match: { email: "john.doe@example.com" } },
    {
        $project: {
            employee_id: 1,
            first_name: 1,
            last_name: 1,
            phone_number: 1,
            email: 1,
            role: 1,
            password: 1,
            passwordMatches: { $eq: ["$password", "Password123"] }
        }
    },
    { $match: { passwordMatches: true } },
    { $limit: 1 }
]);

db.cases.find({
    $or: [
        { agent_id: "A001" },
        { supervisor_id: "S001" }
    ]
}).toArray();

db.cases.findOne({ case_id: "K001" });

db.actions.find({ case_id: "K001" }).sort({ timestamp: 1 }).toArray();

db.cases.find({
    supervisor_id: "S001",
    is_archived: { $ne: true }
}).sort({ creation_date: -1 }).toArray();

db.clients.findOne({ client_id: "C001" });
db.employees.findOne({ employee_id: "E001" });

db.actions.insertOne({
    action_id: "ACT001",
    case_id: "K001",
    action_type: "Investigation",
    description: "Initial investigation of case.",
    timestamp: new Date(),
    duration_minutes: 45,
    is_reviewed: false
});

db.clients.insertOne({
    client_id: "C002",
    first_name: "Jane",
    last_name: "Smith",
    phone_number: "01122334455",
    date_of_birth: ISODate("1995-05-10"),
    sex: "F",
    email: "jane.smith@example.com",
    primary_address: {
        street: "45 Elm St",
        city: "Alexandria",
        country: "Egypt"
    }
});

db.actions.updateOne(
    { action_id: "ACT001" },
    { $set: { is_reviewed: true } }
);

db.cases.updateOne(
    { case_id: "K001" },
    { $set: { is_archived: true } }
);

db.actions.deleteMany({ case_id: "K001" });

db.cases.updateOne(
    { case_id: "K001" },
    {
        $set: {
            title: "Updated Case Title",
            description: "Updated description text",
            priority: "low",
            last_modified: new Date()
        }
    }
);

db.cases.updateOne(
    { case_id: "K001" },
    { $set: { status: "closed", last_modified: new Date() } }
);

var lastCase = db.cases.aggregate([
    { $project: { num: { $toInt: { $substr: ["$case_id", 1, -1] } } } },
    { $sort: { num: -1 } },
    { $limit: 1 }
]).toArray();

var nextNum = (lastCase.length > 0) ? lastCase[0].num + 1 : 1;
var newCaseId = "K" + nextNum.toString().padStart(3, "0");

var supervisors = db.employees.find({ role: "supervisor" }).toArray();
var supervisorId = supervisors[Math.floor(Math.random() * supervisors.length)].employee_id;

db.cases.insertOne({
    case_id: newCaseId,
    title: "Example Case Title",
    description: "This is a test case description.",
    priority: "high",
    status: "open",
    case_origin: "email",
    creation_date: new Date(),
    tags: [],
    client_id: "C001",
    agent_id: "A001",
    supervisor_id: supervisorId
});
