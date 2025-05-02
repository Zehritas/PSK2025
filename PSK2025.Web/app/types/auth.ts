export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  userId: string;
  refreshToken: string;
}

export interface RegisterRequest {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
}

export interface RegisterResponse {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
}

export interface RefreshRequest {
  token: string;
}

export interface RefreshResponse {
  token: string;
  userId: string;
  refreshToken: string;
}
