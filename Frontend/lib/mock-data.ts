import type { Employee, CreateEmployeeDto, UpdateEmployeeDto } from "./types"

// Mock employee data
const mockEmployees: Employee[] = [
  {
    id: 1,
    firstName: "Sarah",
    lastName: "Johnson",
    email: "sarah.johnson@company.com",
    position: "Senior Software Engineer",
  },
  {
    id: 2,
    firstName: "Michael",
    lastName: "Chen",
    email: "michael.chen@company.com",
    position: "Product Manager",
  },
  {
    id: 3,
    firstName: "Emily",
    lastName: "Rodriguez",
    email: "emily.rodriguez@company.com",
    position: "UX Designer",
  },
  {
    id: 4,
    firstName: "James",
    lastName: "Williams",
    email: "james.williams@company.com",
    position: "Frontend Developer",
  },
  {
    id: 5,
    firstName: "Olivia",
    lastName: "Martinez",
    email: "olivia.martinez@company.com",
    position: "Marketing Manager",
  },
]

let nextId = 6

// Simulate API delay
const delay = (ms: number) => new Promise((resolve) => setTimeout(resolve, ms))

export const mockEmployeeApi = {
  getAll: async (): Promise<Employee[]> => {
    await delay(500)
    return [...mockEmployees]
  },

  getById: async (id: number): Promise<Employee> => {
    await delay(300)
    const employee = mockEmployees.find((emp) => emp.id === id)
    if (!employee) {
      throw new Error("Employee not found")
    }
    return { ...employee }
  },

  create: async (data: CreateEmployeeDto): Promise<Employee> => {
    await delay(500)
    const newEmployee: Employee = {
      id: nextId++,
      ...data,
    }
    mockEmployees.push(newEmployee)
    return { ...newEmployee }
  },

  update: async (id: number, data: UpdateEmployeeDto): Promise<Employee> => {
    await delay(500)
    const index = mockEmployees.findIndex((emp) => emp.id === id)
    if (index === -1) {
      throw new Error("Employee not found")
    }
    mockEmployees[index] = { ...mockEmployees[index], ...data }
    return { ...mockEmployees[index] }
  },

  delete: async (id: number): Promise<void> => {
    await delay(500)
    const index = mockEmployees.findIndex((emp) => emp.id === id)
    if (index === -1) {
      throw new Error("Employee not found")
    }
    mockEmployees.splice(index, 1)
  },
}
