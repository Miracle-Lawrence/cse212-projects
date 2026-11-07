/// <summary>
/// Maintain a Customer Service Queue.  Allows new customers to be 
/// added and allows customers to be serviced.
/// </summary>
public class CustomerService {
    public static void Run() {
        // Example code to see what's in the customer service queue:
        // var cs = new CustomerService(10);
        // Console.WriteLine(cs);

        // Test Cases

        // Test 1
        // Scenario: Create a queue with a valid maximum size.
        // Expected Result: The maximum size should be 5.
        Console.WriteLine("Test 1");

        RunTest("Valid maximum queue size", () => {
            var cs = new CustomerService(5);
            Assert(cs.ToString().Contains("max_size=5"),
                "Maximum size should be 5.");
        });

        // Defect(s) Found: None

        Console.WriteLine("=================");

        // Test 2
        // Scenario: Create a queue with a size of 0.
        // Expected Result: The maximum size should default to 10.
        Console.WriteLine("Test 2");

        RunTest("Zero queue size defaults to 10", () => {
            var cs = new CustomerService(0);
            Assert(cs.ToString().Contains("max_size=10"),
                "Maximum size should default to 10.");
        });

        // Defect(s) Found: None

        Console.WriteLine("=================");

        // Add more Test Cases As Needed Below

        // Test 3
        // Scenario: Create a queue with a negative maximum size.
        // Expected Result: The maximum size should default to 10.
        Console.WriteLine("Test 3");

        RunTest("Negative queue size defaults to 10", () => {
            var cs = new CustomerService(-5);
            Assert(cs.ToString().Contains("max_size=10"),
                "Negative maximum size should default to 10.");
        });

        Console.WriteLine("=================");

        // Test 4
        // Scenario: Add a new customer to the queue.
        // Expected Result: The customer should be added successfully.
        Console.WriteLine("Test 4");

        RunTest("Add a new customer", () => {
            var cs = new CustomerService(5);

            AddCustomer(cs, "John", "ACC001", "Account issue");

            Assert(cs.ToString().Contains("John (ACC001)"),
                "Customer should be added.");

            Assert(cs.ToString().Contains("size=1"),
                "Queue should contain one customer.");
        });

        Console.WriteLine("=================");

        // Test 5
        // Scenario: Add customers until the queue reaches its maximum size.
        // Expected Result: All customers should be added successfully.
        Console.WriteLine("Test 5");

        RunTest("Fill queue to maximum capacity", () => {
            var cs = new CustomerService(2);

            AddCustomer(cs, "John", "ACC001", "Account issue");
            AddCustomer(cs, "Mary", "ACC002", "Payment issue");

            Assert(cs.ToString().Contains("size=2 max_size=2"),
                "Queue should contain exactly two customers.");
        });

        Console.WriteLine("=================");

        // Test 6
        // Scenario: Add a customer when the queue is already full.
        // Expected Result: An error message should be displayed and the customer should not be added.
        Console.WriteLine("Test 6");

        RunTest("Reject customer when queue is full", () => {
            var cs = new CustomerService(2);

            AddCustomer(cs, "John", "ACC001", "Account issue");
            AddCustomer(cs, "Mary", "ACC002", "Payment issue");

            var output = AddCustomer(cs, "Peter", "ACC003", "Login issue");

            Assert(output.Contains("Maximum Number of Customers in Queue."),
                "An error message should be displayed.");

            Assert(cs.ToString().Contains("size=2 max_size=2"),
                "Queue should not exceed its maximum size.");

            Assert(!cs.ToString().Contains("Peter"),
                "Peter should not be added.");
        });

        Console.WriteLine("=================");

        // Test 7
        // Scenario: Serve multiple customers from the queue.
        // Expected Result: Customers should be served in FIFO order.
        Console.WriteLine("Test 7");

        RunTest("Serve customers in FIFO order", () => {
            var cs = new CustomerService(5);

            AddCustomer(cs, "John", "ACC001", "Account issue");
            AddCustomer(cs, "Mary", "ACC002", "Payment issue");
            AddCustomer(cs, "Peter", "ACC003", "Login issue");

            var first = ServeCustomer(cs);
            var second = ServeCustomer(cs);
            var third = ServeCustomer(cs);

            Assert(first.Contains("John (ACC001)"),
                "John should be served first.");

            Assert(second.Contains("Mary (ACC002)"),
                "Mary should be served second.");

            Assert(third.Contains("Peter (ACC003)"),
                "Peter should be served third.");

            Assert(cs.ToString().Contains("size=0"),
                "Queue should be empty after serving all customers.");
        });

        Console.WriteLine("=================");

        // Test 8
        // Scenario: Serve a customer when the queue is empty.
        // Expected Result: An error message should be displayed.
        Console.WriteLine("Test 8");

        RunTest("Serve customer from an empty queue", () => {
            var cs = new CustomerService(5);

            var output = ServeCustomer(cs);

            Assert(output.Contains("No Customers in the queue"),
                "An error message should be displayed.");
        });

        Console.WriteLine("=================");

        // Helper methods used by the test cases

        void RunTest(string name, System.Action test) {
            Console.WriteLine(name);

            try {
                test();
                Console.WriteLine("PASSED");
            }
            catch (System.Exception ex) {
                Console.WriteLine("FAILED: " + ex.Message);
            }
        }

        void Assert(bool condition, string message) {
            if (!condition)
                throw new System.Exception(message);
        }

        string AddCustomer(CustomerService cs, string name, string accountId, string problem) {
            var originalInput = Console.In;
            var originalOutput = Console.Out;
            var capturedOutput = new System.IO.StringWriter();

            try {
                Console.SetIn(new System.IO.StringReader(
                    $"{name}{Environment.NewLine}{accountId}{Environment.NewLine}{problem}"));

                Console.SetOut(capturedOutput);

                cs.AddNewCustomer();

                return capturedOutput.ToString();
            }
            finally {
                Console.SetIn(originalInput);
                Console.SetOut(originalOutput);
            }
        }

        string ServeCustomer(CustomerService cs) {
            var originalOutput = Console.Out;
            var capturedOutput = new System.IO.StringWriter();

            try {
                Console.SetOut(capturedOutput);

                cs.ServeCustomer();

                return capturedOutput.ToString();
            }
            finally {
                Console.SetOut(originalOutput);
            }
        }
    }
    private readonly List<Customer> _queue = new();
    private readonly int _maxSize;

    public CustomerService(int maxSize) {
        if (maxSize <= 0)
            _maxSize = 10;
        else
            _maxSize = maxSize;
    }

    /// <summary>
    /// Defines a Customer record for the service queue.
    /// This is an inner class.  Its real name is CustomerService.Customer
    /// </summary>
    private class Customer {
        public Customer(string name, string accountId, string problem) {
            Name = name;
            AccountId = accountId;
            Problem = problem;
        }

        private string Name { get; }
        private string AccountId { get; }
        private string Problem { get; }

        public override string ToString() {
            return $"{Name} ({AccountId})  : {Problem}";
        }
    }

    /// <summary>
    /// Prompt the user for the customer and problem information.  Put the 
    /// new record into the queue.
    /// </summary>
    private void AddNewCustomer() {
        // Verify there is room in the service queue
        if (_queue.Count >= _maxSize) {
            Console.WriteLine("Maximum Number of Customers in Queue.");
            return;
        }

        Console.Write("Customer Name: ");
        var name = Console.ReadLine()!.Trim();
        Console.Write("Account Id: ");
        var accountId = Console.ReadLine()!.Trim();
        Console.Write("Problem: ");
        var problem = Console.ReadLine()!.Trim();

        // Create the customer object and add it to the queue
        var customer = new Customer(name, accountId, problem);
        _queue.Add(customer);
    }

    /// <summary>
    /// Dequeue the next customer and display the information.
    /// </summary>
    private void ServeCustomer() {
        if (_queue.Count <= 0) // Defect 2 - Need to check queue length
        {
            Console.WriteLine("No Customers in the queue");
        }
        else {
            // Need to read and save the customer before it is deleted from the queue
            var customer = _queue[0];
            _queue.RemoveAt(0); // Defect 1 - Delete should be done after
            Console.WriteLine(customer);
        }
    }

    /// <summary>
    /// Support the WriteLine function to provide a string representation of the
    /// customer service queue object. This is useful for debugging. If you have a 
    /// CustomerService object called cs, then you run Console.WriteLine(cs) to
    /// see the contents.
    /// </summary>
    /// <returns>A string representation of the queue</returns>
    public override string ToString() {
        return $"[size={_queue.Count} max_size={_maxSize} => " + string.Join(", ", _queue) + "]";
    }
}