import { API_URL } from "../constants";


interface IAuthorize {
    Logout: () => IBuild;
    Register: () => IBuild;
    Login: () => IBuild;
}

interface IBuild {
    Build: () => string;
}

/**
 * Helps building API endpoint paths easier.
 */
class UriBuilder implements IAuthorize, IBuild {
    private _endpoint: string;
    private constructor(initialValue: string) {
        this._endpoint = initialValue;
    }

    /**
     * Instantiates a new UriBuilder object.
     */
    static use(): IAuthorize  {
        return new UriBuilder('');
    }

    /**
     * Selects the login API endpoint.
     */
    Login(): IBuild {
        this._endpoint = '/api/Authentication/LoginUser';
        return this;
    };
    
    /**
     * Selects the register API endpoint.
     */
    Register(): IBuild {
        this._endpoint = '/api/Authentication/RegisterUser';
        return this;
    };
    
    /**
     * Selects the logout API endpoint.
     */
    Logout(): IBuild {
        this._endpoint = '/api/Authentication/LogoutUser';
        return this;
    }

    /**
     * Combines the API URL with the selected endpoint.
     * @returns The URL as string.
     */
    Build(): string {
        return new URL(this._endpoint, API_URL).href;
    }
}

export default UriBuilder;