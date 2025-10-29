import axios from "axios"
import type { Employee, CreateEmployeeDto, UpdateEmployeeDto } from "./types"

const API_BASE_URL = process.env.NEXT_PUBLIC_API_URL?.replace(/\/+$/, "")

// Axios instance (sem token mock)
export const apiClient = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    "Content-Type": "application/json",
  },
})

// Anexa Authorization dinamicamente a partir do localStorage
apiClient.interceptors.request.use((config) => {
  if (typeof window !== "undefined") {
    const token = localStorage.getItem("auth_token")
    if (token) {
      config.headers = config.headers ?? {}
      config.headers["Authorization"] = `Bearer ${token}`
    }
  }
  return config
})

// Helper para normalizar o DTO vindo da API
function normalizeEmployeeDto(dto: any): Employee {
  return {
    id: dto.id,
    firstName: dto.firstName,
    lastName: dto.lastName,
    email: dto.email,
    position: dto.position,
  }
}

// API de Employees conectada ao backend
export const employeeApi = {
  getAll: async (): Promise<Employee[]> => {
    const { data } = await apiClient.get("/employees")
    const list = Array.isArray(data) ? data : Array.isArray((data as any)?.employees) ? (data as any).employees : []
    return list.map(normalizeEmployeeDto)
  },

  getById: async (id: number): Promise<Employee> => {
    const { data } = await apiClient.get(`/employees/${id}`)
    const dto = (data as any)?.employee ?? data
    if (!dto) throw new Error("Employee not found")
    return normalizeEmployeeDto(dto)
  },

  create: async (employee: CreateEmployeeDto): Promise<Employee> => {
    const { data } = await apiClient.post("/employees", employee)
    const dto = (data as any)?.employee ?? data
    return normalizeEmployeeDto(dto)
  },

  update: async (id: number, employee: UpdateEmployeeDto): Promise<Employee> => {
    if (!employee.firstName || !employee.lastName || !employee.email || !employee.position) {
      throw new Error("Update requer todos os campos: firstName, lastName, email, position")
    }
    const payload = { id, ...employee }
    const { data } = await apiClient.put(`/employees/${id}`, payload)
    const dto = (data as any)?.employee ?? data
    return normalizeEmployeeDto(dto)
  },

  delete: async (id: number): Promise<void> => {
    await apiClient.delete(`/employees/${id}`)
  },
}
