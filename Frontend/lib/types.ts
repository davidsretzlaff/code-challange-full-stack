export interface Employee {
  id: number
  firstName: string
  lastName: string
  email: string
  position: string
}

export interface CreateEmployeeDto {
  firstName: string
  lastName: string
  email: string
  position: string
}

export interface UpdateEmployeeDto extends Partial<CreateEmployeeDto> {}
