import {jwtDecode} from 'jwt-decode';

interface TokenPayload {
    exp: number;                    // Время истечения токена
    iat: number;                    // Время выдачи токена
    name: string;                   // Имя пользователя (ClaimTypes.Name)
    role: string;                   // Роль пользователя (ClaimTypes.Role)
    [key: string]: any; 
}

class TokenService {
    private readonly userResponseKey = 'userResponse';
  
    // Сохранить объект пользователя (включая токен) в локальное хранилище
  saveUserResponse(userResponse: any): void {
    localStorage.setItem(this.userResponseKey, JSON.stringify(userResponse));
  }
    // Получить токен из локального хранилища
    getToken(): string | null {
        const userResponse = localStorage.getItem(this.userResponseKey);
        if (userResponse) {
          const parsedUserResponse = JSON.parse(userResponse);
          return parsedUserResponse.token || null;
        }
        return null;
      }

      getUserResponse(): any | null {
        const userResponse = localStorage.getItem(this.userResponseKey);
        return userResponse ? JSON.parse(userResponse) : null;
      }
  removeUserResponse(): void {
    localStorage.removeItem(this.userResponseKey);
  }
     // Удалить токен из объекта userResponse в локальном хранилище
  removeToken(): void {
    const userResponse = this.getUserResponse();
    if (userResponse) {
      delete userResponse.token;
      this.saveUserResponse(userResponse);
    }
  }
  
    // Декодировать токен
    decodeToken<T = TokenPayload>(token: string): T | null {
        try {
          return jwtDecode<T>(token);
        } catch (error) {
          console.error('Invalid token:', error);
          return null;
        }
      }
   
    // Добавить токен в заголовки
    getAuthHeaders(): { [key: string]: string } {
        const token = this.getToken();
        return {
          'Content-Type': 'application/json',
          'Authorization': token ? `Bearer ${token}` : ''
        };
      }
  }

const tokenService = new TokenService();
export default tokenService;