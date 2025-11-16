/* eslint-disable no-debugger */
export default class Service {
    static auth_api_domain = import.meta.env.VITE_AUTH0_API_DOMAIN;
    static auth_api_id = import.meta.env.VITE_AUTH0_API_CLIENT;
    static auth_api_secret = import.meta.env.VITE_AUTH0_API_SECRET;
    static auth_api_audience = import.meta.env.VITE_AUTH0_API_AUDIENCE;

    static api_domain = import.meta.env.VITE_API_DOMAIN;

    static async getToken(): Promise<string | null> {
        try {
            const response = await fetch(
                Service.auth_api_domain,
                {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                    },
                    body: JSON.stringify({
                        client_id: Service.auth_api_id,
                        client_secret: Service.auth_api_secret,
                        audience: Service.auth_api_audience,
                        grant_type: "client_credentials",
                    }),
                }
            );

            const data = await response.json();

            localStorage.setItem("token", data.access_token);
            return data.access_token ?? null;
        } catch (error) {
            console.error("Auth0 Token Error:", error);
            return null;
        }
    }

    static async getHeaders(): Promise<{ [key: string]: string }> {
        let token = localStorage.getItem("token");

        // ถ้าไม่มี token → ขอใหม่
        if (!token) {
            token = await Service.getToken();

            if (token) {
                localStorage.setItem("token", token);
            }
        }

        const headers = {
            "Content-Type": "application/json",
            "Authorization": `Bearer ${token}`,
        };

        return headers;
    }

    static async refreshToken(): Promise<string | null> {
        console.warn("Token expired. Refreshing token...");

        // Request new token
        const newToken = await Service.getToken();

        if (newToken) {
            localStorage.setItem("token", newToken);
        }

        return newToken;
    }

    static async getUser(email: string = "",isRetry: boolean = false): Promise<object[] | null> {
        try {
            const headers = await Service.getHeaders();

            const response = await fetch(
                Service.api_domain + "UserController/GetUser" + (email ? `?email=${email}` : ""),
                {
                    method: "GET",
                    headers: headers
                }
            );

            if (response.status === 401 && !isRetry) {
                await Service.refreshToken();
                return await Service.getUser(email, true);
            }

            if (!response.ok) {
                console.error("API Error:", response.status);
                return null;
            }

            const data = await response.json();
            return data ?? null;
        } catch (error) {
            console.error("Auth0 Token Error:", error);
            return null;
        }
    }

    static async getLeaderboard(email: string = "",isRetry: boolean = false): Promise<object[] | null> {
        try {
            const headers = await Service.getHeaders();

            const response = await fetch(
                Service.api_domain + "UserController/GetLeaderboard" + (email ? `?email=${email}` : ""),
                {
                    method: "GET",
                    headers: headers
                }
            );

            if (response.status === 401 && !isRetry) {
                await Service.refreshToken();
                return await Service.getUser(email, true);
            }

            if (!response.ok) {
                console.error("API Error:", response.status);
                return null;
            }

            const data = await response.json();
            return data ?? null;
        } catch (error) {
            console.error("Auth0 Token Error:", error);
            return null;
        }
    }

    static async CreateUser(email: string,isRetry: boolean = false): Promise<string | null> {
        try {
            const headers = await Service.getHeaders();

            const response = await fetch(
                Service.api_domain + "UserController/CreateUser",
                {
                    method: "POST",
                    headers: headers,
                    body: JSON.stringify({
                        email: email,
                    }),
                }
            );

            if (response.status === 401 && !isRetry) {
                await Service.refreshToken();
                return await Service.CreateUser(email, true);
            }

            if (!response.ok) {
                console.error("API Error:", response.status);
                return null;
            }

            const data = await response.json();
            return data ?? null;
        } catch (error) {
            console.error("Auth0 Token Error:", error);
            return null;
        }
    }

    static async UpdateScore(id: string, score: number, isRetry: boolean = false): Promise<string | null> {
        try {
            const headers = await Service.getHeaders();

            const response = await fetch(
                Service.api_domain + "ScoreController/UpdateScore",
                {
                    method: "POST",
                    headers: headers,
                    body: JSON.stringify({
                        id: id,
                        score: score
                    }),
                }
            );

            if (response.status === 401 && !isRetry) {
                await Service.refreshToken();
                return await Service.UpdateScore(id, score, true);
            }

            if (!response.ok) {
                console.error("API Error:", response.status);
                return null;
            }

            const data = await response.json();
            return data ?? null;
        } catch (error) {
            console.error("Auth0 Token Error:", error);
            return null;
        }
    }

    static async checkWinCount(userId: string,isRetry: boolean = false): Promise<number | null> {
        try {
            const headers = await Service.getHeaders();

            const response = await fetch(
                Service.api_domain + "ScoreController/CheckWinCount?userId=" + userId,
                {
                    method: "GET",
                    headers: headers
                }
            );

            if (response.status === 401 && !isRetry) {
                await Service.refreshToken();
                return await Service.checkWinCount(userId, true);
            }

            if (!response.ok) {
                console.error("API Error:", response.status);
                return null;
            }

            const data = await response.json();
            return data ?? null;
        } catch (error) {
            console.error("Auth0 Token Error:", error);
            return null;
        }
    }
}