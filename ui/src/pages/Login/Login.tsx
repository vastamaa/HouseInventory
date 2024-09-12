import React, { ChangeEvent, MouseEvent, useState } from 'react'
import './Login.css'
import Input from '../../components/input/Input';
import Title from '../../components/title/Title';
import Button from '../../components/button/Button';

interface IUserLogin {
  emailAddress: string;
  password: string;
}

const Login = (): JSX.Element => {
  const [userLogin, setUserLogin] = useState<IUserLogin>({ emailAddress: '', password: '' });

  const handleButtonClick = (event: MouseEvent<HTMLInputElement>) => {
    // You'll update this function later...
    // Send data to the api.
    event.preventDefault();
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
      <Input onHandleInputChange={handleInputChange} defaultText='Enter your e-mail here' value={userLogin.emailAddress} type={'email'} />
      <br />
      <Input onHandleInputChange={handleInputChange} defaultText='Enter your password here' value={userLogin.password} type='password' />
      <br />
      <Button onHandleButtonClick={handleButtonClick} />
    </div>
  )
}

export default Login;