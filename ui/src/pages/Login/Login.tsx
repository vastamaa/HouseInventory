import React, { ChangeEvent, MouseEvent, useState } from 'react'
import './Login.css'
import Input from '../../components/input/Input';
import Title from '../../components/title/Title';
import Button from '../../components/button/Button';
import { useHttpRequestService } from '../../contexts/HttpRequestServiceContext';
import UriBuilder from '../../utils/UriBuilder';

interface IUserLogin {
  email: string;
  password: string;
  rememberMe?: boolean | null;
}

const Login = (): JSX.Element => {
  const [userLogin, setUserLogin] = useState<IUserLogin>({ email: '', password: '' });
  const httpRequestService = useHttpRequestService();

  const handleLogin = async (event: MouseEvent<HTMLInputElement>) => {
    event.preventDefault();

    const loginUri = UriBuilder.use().Login().Build();
    try {
      const response = await httpRequestService.PostResourceAsync<IUserLogin>(loginUri, userLogin);
      console.log('Response value:', response);
    }
    catch (error) {
      console.log('Error fetching data:', error);
    }
  }

  const handleInputChange = (event: ChangeEvent<HTMLInputElement>): void => {
    const { name, value } = event.target;

    setUserLogin((prevState) => ({
      ...prevState,
      [name]: value
    }));
  };

  return (
    <div className='main-container'>
      <Title title={'Login'} />
      <br />
      <Input onHandleInputChange={handleInputChange} defaultText='Enter your e-mail here' value={userLogin.email} name='email' type={'email'} />
      <br />
      <Input onHandleInputChange={handleInputChange} defaultText='Enter your password here' value={userLogin.password} name='password' type='password' />
      <br />
      <Button onHandleButtonClick={handleLogin} />
    </div>
  )
}

export default Login;