import React from 'react';

export function Login(props){
    return (
        <div className='login-cont'>
            <input type='text' className='login-cont__textbox'/>
            <input type='password' className='login-cont__textbox'/>
            <button>
                Log In
            </button>
        </div>
    )
}