export interface LoginCredentials {
  email: string
  password: string
}

export interface AuthResponse {
  success: boolean
  message?: string
  token?: string
}

// Mock authentication - always succeeds for now
export const mockAuthApi = {
  login: async (credentials: LoginCredentials): Promise<AuthResponse> => {
    // Simulate API delay
    await new Promise((resolve) => setTimeout(resolve, 800))

    // For now, always return success
    // Later, you can add real validation here
    return {
      success: true,
      token: "mock-jwt-token-" + Date.now(),
    }
  },

  logout: () => {
    if (typeof window !== "undefined") {
      localStorage.removeItem("auth_token")
      localStorage.removeItem("user_email")
    }
  },

  isAuthenticated: (): boolean => {
    if (typeof window !== "undefined") {
      return !!localStorage.getItem("auth_token")
    }
    return false
  },

  getToken: (): string | null => {
    if (typeof window !== "undefined") {
      return localStorage.getItem("auth_token")
    }
    return null
  },
}
