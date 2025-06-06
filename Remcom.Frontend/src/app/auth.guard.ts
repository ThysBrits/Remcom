import { Injectable } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const authGuard: CanActivateFn = (route, state) => {
  const token = localStorage.getItem('token');
  if (!token) {
    // Not logged in, redirect to login
    window.location.href = '/login';
    return false;
  }
  return true;
};
